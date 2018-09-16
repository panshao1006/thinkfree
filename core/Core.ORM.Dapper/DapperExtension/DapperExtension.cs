using Core.Common.ConfigManager;
using Core.Log;
using Core.ORM.Dapper.Enum;
using Core.ORM.Dapper.Filter;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.ORM.Dapper
{
    /// <summary>
    /// dapper的扩展
    /// </summary>
    public class DapperExtension
    {
        private string _defaultConnectionStringKey = "ConnectionString";

        private string _defaultDatabaseTypeKey = "DBType";

        private string _connectionString;

        private DBType _dbType;

        //所有model对象的主键值
        private string _primaryKey = "id";

        private static Dictionary<string, Dictionary<SqlStringType, string>> _sqlStringCache;

        /// <summary>
        /// 对象的放射类型
        /// </summary>
        private static Dictionary<string, PropertyInfo[]> _objectPropertyCache;

        public delegate void ExecutedEventHandler(string sqlcommand);

        public ExecutedEventHandler ExecutedEvent;


        public DapperExtension()
        {
            _sqlStringCache = _sqlStringCache ?? new Dictionary<string, Dictionary<SqlStringType, string>>();

            _objectPropertyCache = _objectPropertyCache ?? new Dictionary<string, PropertyInfo[]>();

            _connectionString = ConfigurtaionManager.AppSettings(_defaultConnectionStringKey);

            var dbType = ConfigurtaionManager.AppSettings(_defaultDatabaseTypeKey);

            if (!string.IsNullOrWhiteSpace(dbType))
            {
                _dbType = (DBType)int.Parse(dbType);
            }
        }

        public DapperExtension(string connectString, DBType dbType)
        {
            _connectionString = connectString;

            _dbType = dbType;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        private IDbConnection GetConnection()
        {
            IDbConnection connection = null;

            switch (_dbType)
            {
                case DBType.MYSQL:
                    connection = new MySqlConnection(_connectionString);
                    connection.Open();
                    break;
            }

            return connection;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<T> Query<T>(FilterBase filter)
        {
            using (IDbConnection conn = GetConnection())
            {
                var sqlCommand = GetSqlCommand<T>(SqlStringType.Select, filter, default(T));

                List<T> result = conn.Query<T>(sqlCommand.Sql, sqlCommand.Parameters).ToList();

                if (ExecutedEvent != null)
                {
                    string sqlCommandString = JsonConvert.SerializeObject(sqlCommand);
                    ExecutedEvent(sqlCommandString);
                }

                return result;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t)
        {
            using (IDbConnection conn = GetConnection())
            {
                var sqlCommand = GetSqlCommand<T>(SqlStringType.Insert, null, t);

                int effRows = conn.Execute(sqlCommand.Sql, sqlCommand.Parameters);

                if (ExecutedEvent != null)
                {
                    string sqlCommandString = JsonConvert.SerializeObject(sqlCommand);
                    ExecutedEvent(sqlCommandString);
                }

                //如果执行成功，返回对象，如果不成功返回空
                if (effRows > 0)
                {
                    return t;
                }

                return default(T);
            }
        }

        public bool Delete<T>(T t)
        {
            using (IDbConnection conn = GetConnection())
            {
                var sqlCommand = GetSqlCommand<T>(SqlStringType.Delete, null, t);

                int effRows = conn.Execute(sqlCommand.Sql, sqlCommand.Parameters);

                if (ExecutedEvent != null)
                {
                    string sqlCommandString = JsonConvert.SerializeObject(sqlCommand);
                    ExecutedEvent(sqlCommandString);
                }

                //如果执行成功，返回对象，如果不成功返回空

                return effRows > 0;
            }
        }


        public bool Update<T>(T value)
        {
            using (IDbConnection conn = GetConnection())
            {
                var sqlCommand = GetSqlCommand<T>(SqlStringType.Update, null, value);

                int effRows = conn.Execute(sqlCommand.Sql, sqlCommand.Parameters);

                if (ExecutedEvent != null)
                {
                    string sqlCommandString = JsonConvert.SerializeObject(sqlCommand);
                    ExecutedEvent(sqlCommandString);
                }
                //如果执行成功，返回对象，如果不成功返回空

                return effRows > 0;
            }
        }

        /// <summary>
        /// 获取sqlstirng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlType">insert ， update ， delete ， select</param>
        /// <param name="value">需要传入的对象 insert和update可用</param>
        /// <returns></returns>
        private SqlCommand GetSqlCommand<T>(SqlStringType sqlStringType, FilterBase filter, T value)
        {
            SqlCommand result = null;

            string baseSqlString = string.Empty;

            Dictionary<SqlStringType, string> tempSqlStringDic = null;

            if (_sqlStringCache.TryGetValue(typeof(T).FullName, out tempSqlStringDic) && tempSqlStringDic.TryGetValue(sqlStringType, out baseSqlString))
            {
                //如果没有，标识需要重新初始化该model的sqlstring
                if (string.IsNullOrWhiteSpace(baseSqlString))
                {
                    //重新初始化数据
                    throw new NullReferenceException("没有找到model下面的sql语句");
                }
            }
            else
            {
                //如果没有，需要重新初始化该model的sqlstring
                baseSqlString = InitialSqlString<T>(sqlStringType, value);
            }

            //只有查询，才做过滤条件的处理
            if (sqlStringType == SqlStringType.Select)
            {
                result = GetFilterSqlCommand(baseSqlString, filter, sqlStringType);
            }
            else if (sqlStringType == SqlStringType.Insert)
            {
                result = GetInsertSqlCommand<T>(baseSqlString, value);
            }
            else if (sqlStringType == SqlStringType.Delete)
            {
                result = GetDeleteSqlCommand<T>(baseSqlString, value);
            }
            else if (sqlStringType == SqlStringType.Update)
            {
                result = GetUpdateSqlCommand<T>(baseSqlString, value);
            }

            return result;
        }


        /// <summary>
        /// 获取插入的sqlcommand
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseSqlString"></param>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private SqlCommand GetInsertSqlCommand<T>(string baseSqlString, T value)
        {
            SqlCommand sqlCommand = new SqlCommand(baseSqlString);

            var propertyInfos = GetPropertyInfos(typeof(T));

            string result = string.Empty;

            int index = 0;
            foreach (var propertyInfo in propertyInfos)
            {
                object propertyValue = null;

                if (propertyInfo.Name.ToLower() == _primaryKey)
                {
                    propertyValue = Guid.NewGuid().ToString();
                    propertyInfo.SetValue(value, propertyValue);
                }
                else
                {
                    propertyValue = propertyInfo.GetValue(value);
                }

                sqlCommand.Parameters.Add("@Parmeter" + index, propertyValue);

                index++;
            }

            return sqlCommand;

        }


        /// <summary>
        /// 获取更新的sqlcommand
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseSqlString"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private SqlCommand GetUpdateSqlCommand<T>(string baseSqlString, T value)
        {
            SqlCommand sqlCommand = new SqlCommand(baseSqlString);

            var propertyInfos = GetPropertyInfos(typeof(T));

            string result = string.Empty;

            int index = 0;

            object primaryKeyValue = null;

            foreach (var propertyInfo in propertyInfos)
            {
                object propertyValue = null;

                if (propertyInfo.Name.ToLower() == _primaryKey)
                {
                    primaryKeyValue = propertyInfo.GetValue(value);

                    if (primaryKeyValue == null)
                    {
                        throw new ArgumentNullException("model的id不能为空");
                    }

                    continue;
                }

                propertyValue = propertyInfo.GetValue(value);

                sqlCommand.Parameters.Add("@Parmeter" + index, propertyValue);

                index++;
            }

            sqlCommand.Parameters.Add("@Parmeter" + index, primaryKeyValue);

            return sqlCommand;

        }


        /// <summary>
        /// 获取删除的sqlcommand
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseSqlString"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private SqlCommand GetDeleteSqlCommand<T>(string baseSqlString, T value)
        {
            SqlCommand sqlCommand = new SqlCommand(baseSqlString);

            var propertyInfo = GetProperty(typeof(T), _primaryKey);

            string result = string.Empty;

            sqlCommand.Parameters.Add("@Parmeter0", propertyInfo.GetValue(value));

            return sqlCommand;

        }


        /// <summary>
        /// 获取sql的过滤条件字符串
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="filter"></param>
        /// <param name="sqlStringType"></param>
        /// <returns></returns>
        private SqlCommand GetFilterSqlCommand(string sqlString, FilterBase filter, SqlStringType sqlStringType)
        {
            SqlCommand result = new SqlCommand(sqlString);

            if (filter == null)
            {
                return result;
            }

            //查询字段处理
            if (filter.QueryFields != null && filter.QueryFields.Count > 0)
            {
                string queryFieldContactString = string.Join(',', filter.QueryFields);

                result.Sql = result.Sql.Replace("*", queryFieldContactString);
            }

            //查询条件的处理
            if (filter.Conditons != null && filter.Conditons.Count > 0)
            {
                result = GetConditionSqlCommand(result.Sql, filter.Conditons);
            }

            if (filter.FilterSegments != null && filter.FilterSegments.Count > 0)
            {
                result = GetFilterSegmentSqlCommand(result.Sql, filter.FilterSegments);
            }

            //处理分页和排序
            result = GetPageAndSortSqlCommand(result , filter);

            return result;
        }

        /// <summary>
        /// 处理分页和排序
        /// </summary>
        /// <param name="result"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private SqlCommand GetPageAndSortSqlCommand(SqlCommand result, FilterBase filter)
        {
            

            if(!string.IsNullOrWhiteSpace(filter.SortField) && !string.IsNullOrWhiteSpace(filter.Sort))
            {
                result.Sql += $" ORDER BY {filter.SortField} {filter.Sort} ";
            }

            //排序,只要有一个不为0，表示要排序
            if (filter.PageIndex != 0 || filter.PageSize != 0)
            {
                switch (_dbType)
                {
                    case DBType.MYSQL:
                        result.Sql += $" limit {filter.PageIndex},{filter.PageSize} ";
                        break;
                }

            }

            return result;
        }


        /// <summary>
        /// 获取过滤块sqlcommand
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filterSegment"></param>
        /// <returns></returns>
        private SqlCommand GetFilterSegmentSqlCommand(string baseSqlString, List<FilterSegment> filterSegments)
        {
            SqlCommand sqlCommand = new SqlCommand();

            List<SqlCommand> tempSqlCommands = new List<SqlCommand>();

            StringBuilder sqlBuilder = new StringBuilder();

            //原始的查询条件是否包括where
            bool isContainerWhere = baseSqlString.ToLower().IndexOf("where") > 0;

            for (int i= 0;i<filterSegments.Count;i++)
            {
                FilterSegment segment = filterSegments[i];

                SqlCommand segmentSqlCommand = GetConditionSqlCommand("", segment.Conditions, "segment" + i, true);

                if (!isContainerWhere)
                {
                    sqlBuilder.Append($" WHERE ({segmentSqlCommand.Sql})  ");
                    isContainerWhere = true;
                }
                else
                {
                    sqlBuilder.Append($" {segment.OperatorType.ToString("G")} ({segmentSqlCommand.Sql})  ");
                }

                sqlCommand.Parameters.AddDynamicParams(segmentSqlCommand.Parameters);
            }

            if (sqlBuilder.Length > 0)
            {
                baseSqlString += sqlBuilder.ToString();
                sqlCommand.Sql = baseSqlString;
            }

            return sqlCommand;
        }




        /// <summary>
        /// 获取查询的sqlcommand
        /// </summary>
        /// <param name="baseSqlString">基础sql</param>
        /// <param name="filterConditions"></param>
        /// <param name="parameterPrefix">参数前缀，默认为1</param>
        /// <param name="isSegment">是否条件块</param>
        /// <returns></returns>
        private SqlCommand GetConditionSqlCommand(string baseSqlString, List<FilterCondition> filterConditions , string parameterPrefix = "1", bool isSegment = false)
        {
            var parameters = new Dictionary<string, object>();

            List<string> conditionStringList = new List<string>();

            #region condition的处理
            for (int i = 0; i < filterConditions.Count; i++)
            {
                string parameterName = $"@Parameter_{parameterPrefix}_{i}";

                string conditionString = string.Empty;

                var filterCondition = filterConditions[i];

                switch (filterCondition.LogicType)
                {
                    case LogicType.Equal:
                        conditionString = $" {filterCondition.OperatorType.ToString("G")} ({filterCondition.Field}= {parameterName})";
                        parameters.Add(parameterName, filterCondition.Value);
                        break;

                }

                if (conditionString != null)
                {
                    conditionStringList.Add(conditionString);
                }
            }

            #endregion

            if (conditionStringList.Count > 0)
            {
                //原始的查询条件是否包括where
                bool isContainerWhere = baseSqlString.ToLower().IndexOf("where") > 0;

                //如果不含where
                if (!isSegment && !isContainerWhere)
                {
                    conditionStringList[0] = conditionStringList[0].Replace("AND", "").Replace(" OR ", "");

                    baseSqlString += $" WHERE {string.Join(" ", conditionStringList)}";
                }
                else
                {
                    if (isSegment)
                    {
                        conditionStringList[0] = conditionStringList[0].Replace("AND", "").Replace(" OR ", "");
                    }

                    baseSqlString += $" {string.Join(" ", conditionStringList)}";
                }
            }

            var result = new SqlCommand();

            result.Sql = baseSqlString;

            foreach (var parameter in parameters)
            {
                result.Parameters.Add(parameter.Key, parameter.Value);
            }

            return result;

        }


        /// <summary>
        /// 初始化model的sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="sqlStringType"></param>
        /// <returns></returns>
        private string InitialSqlString<T>(SqlStringType sqlStringType, T value)
        {
            string result = string.Empty;

            switch (sqlStringType)
            {
                case SqlStringType.Select:
                    result = GetSelectSqlString<T>();
                    break;
                case SqlStringType.Insert:
                    result = GetInsertSqlString<T>();
                    break;
                case SqlStringType.Delete:
                    result = GetDeleteSqlString<T>();
                    break;
                case SqlStringType.Update:
                    result = GetUpdateSqlString<T>();
                    break;
            }

            Dictionary<SqlStringType, string> tempSqlStringDic;

            //取不到，重新新增
            if (!_sqlStringCache.TryGetValue(typeof(T).FullName, out tempSqlStringDic))
            {
                tempSqlStringDic = new Dictionary<SqlStringType, string>();

                tempSqlStringDic.Add(sqlStringType, result);

                _sqlStringCache.Add(typeof(T).FullName, tempSqlStringDic);
            }
            else
            {
                tempSqlStringDic = tempSqlStringDic ?? new Dictionary<SqlStringType, string>();

                tempSqlStringDic.Add(sqlStringType, result);
            }



            return result;

        }


        /// <summary>
        /// 获取model对应的表名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetTableName<T>()
        {
            string result = string.Empty;

            Type type = typeof(T);

            var tableNamePoperty = type.GetProperty("TableName");

            //如果没有这个属性，就不进行处理
            if (tableNamePoperty == null)
            {
                throw new ArgumentException("model 不存在TableName的属性");
            }

            string tableName = tableNamePoperty.GetValue(Activator.CreateInstance<T>()).ToString();

            //如果没有这个属性，就不进行处理
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("model的tablename属性为空");
            }

            return tableName;
        }


        /// <summary>
        /// 获取查询的sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStringType"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetSelectSqlString<T>()
        {
            string tableName = GetTableName<T>();

            string result = string.Empty;

            result = $"select * from {tableName}";

            return result;
        }


        /// <summary>
        /// 获取新增的sqlstring
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetInsertSqlString<T>()
        {
            Type t = typeof(T);

            var tableName = GetTableName<T>();

            var propertyInfos = GetPropertyInfos(t);

            string result = string.Empty;

            if (propertyInfos.Length == 0)
            {
                return result;
            }

            //表字段
            List<string> fields = new List<string>();
            //参数
            List<string> parameters = new List<string>();

            int index = 0;
            foreach (var propertyInfo in propertyInfos)
            {
                var propertyField = propertyInfo.Name;

                fields.Add(propertyField);
                parameters.Add("@Parmeter" + index);
                index++;
            }

            result = string.Format(" INSERT INTO {0}({1})VALUES({2}) ", tableName, string.Join(",", fields), string.Join(",", parameters));

            return result;
        }

        /// <summary>
        /// 获取删除的sqlstring
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetDeleteSqlString<T>()
        {
            string tableName = GetTableName<T>();

            string result = string.Empty;

            result = $"DELETE FROM {tableName} WHERE {_primaryKey}=@Parmeter0";

            return result;
        }


        /// <summary>
        /// 获取更新的sqlstring
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetUpdateSqlString<T>()
        {
            Type t = typeof(T);

            var tableName = GetTableName<T>();

            var propertyInfos = GetPropertyInfos(t);

            string result = string.Empty;

            if (propertyInfos.Length == 0)
            {
                return result;
            }

            //表字段
            List<string> fields = new List<string>();


            int index = 0;
            foreach (var propertyInfo in propertyInfos)
            {
                var propertyField = propertyInfo.Name;

                if (propertyField.ToLower() == _primaryKey.ToLower())
                {
                    continue;
                }


                fields.Add($" {propertyField}=@Parmeter{index} ");
                index++;
            }

            result = string.Format("UPDATE {0} SET {1} WHERE id ={2} ", tableName, string.Join(",", fields), "@Parmeter" + index);

            return result;
        }


        /// <summary>
        /// 获取对象的所有反射类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private PropertyInfo[] GetPropertyInfos(Type t)
        {
            string fullName = t.FullName;

            PropertyInfo[] propertyInfos = null;

            if (!_objectPropertyCache.TryGetValue(fullName, out propertyInfos))
            {
                var tempPropertyInfos = t.GetProperties();

                List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();

                foreach (var propertyInfo in tempPropertyInfos)
                {
                    //进行一次过滤,排除掉不是表结构中的属性
                    if (propertyInfo.GetCustomAttribute(typeof(JsonIgnoreAttribute)) != null)
                    {
                        continue;
                    }
                    propertyInfoList.Add(propertyInfo);
                }

                propertyInfos = propertyInfoList.ToArray();

                _objectPropertyCache.Add(fullName, propertyInfos);
            }

            return propertyInfos;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private PropertyInfo GetProperty(Type t, string name)
        {
            var propertyInfos = GetPropertyInfos(t);

            if (propertyInfos == null && propertyInfos.Count() == 0)
            {
                return null;
            }

            var result = propertyInfos.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            return result;
        }


    }
}
