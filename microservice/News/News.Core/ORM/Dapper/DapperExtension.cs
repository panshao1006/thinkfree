using Dapper;
using MySql.Data.MySqlClient;
using News.Model;
using News.Model.Enum;
using News.Model.Filter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace News.Core.ORM.Dapper
{
    /// <summary>
    /// dapper的扩展
    /// </summary>
    public class DapperExtension
    {
        //private IDbConnection _dbConnection;

        private string _defaultConnectionStringKey = "ConnectionString";

        private string _defaultDatabaseTypeKey = "DatabaseType";

        private string _connectionString;

        private string _databaseType;


        private static Dictionary<string, Dictionary<SqlStringType, string>> _sqlStringCache ;


        public DapperExtension()
        {
            _sqlStringCache = _sqlStringCache ?? new Dictionary<string, Dictionary<SqlStringType, string>>();

            _connectionString = ConfigurtaionManager.AppSettings(_defaultConnectionStringKey);

            _databaseType = ConfigurtaionManager.AppSettings(_defaultDatabaseTypeKey);
        }

        public DapperExtension(string connectString , string dataBaseType)
        {
            _connectionString = connectString;

            _databaseType = dataBaseType;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        private IDbConnection GetConnection()
        {
            IDbConnection connection = null;

            switch (_databaseType)
            {
                case "Mysql":
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
                var  sqlCommand = GetSqlCommand<T>(SqlStringType.Select , filter);

                List<T> result = conn.Query<T>(sqlCommand.Sql , sqlCommand.Parameters).ToList();

                return result;
            }
        }



        /// <summary>
        /// 获取sqlstirng
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlType">insert ， update ， delete ， select</param>
        /// <returns></returns>
        private SqlCommand GetSqlCommand<T>(SqlStringType sqlStringType, FilterBase filter)
        {
            SqlCommand result = null;

            string baseSqlString = string.Empty;

            Dictionary<SqlStringType, string> tempSqlStringDic = null;

            if (_sqlStringCache.TryGetValue(typeof(T).FullName, out tempSqlStringDic))
            {
                tempSqlStringDic.TryGetValue(sqlStringType, out baseSqlString);

                //如果没有，标识需要重新初始化该model的sqlstring
                if (string.IsNullOrWhiteSpace(baseSqlString))
                {
                    throw new NullReferenceException("没有找到model下面的sql语句");
                }
            }
            else
            {
                //如果没有，需要重新初始化该model的sqlstring
                baseSqlString = InitSqlString<T>(sqlStringType);
            }

            result = GetFilterSqlCommand(baseSqlString, filter, sqlStringType);

            return result;
        }

        /// <summary>
        /// 获取sql的过滤条件字符串
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="filter"></param>
        /// <param name="sqlStringType"></param>
        /// <returns></returns>
        private SqlCommand GetFilterSqlCommand(string sqlString , FilterBase filter , SqlStringType sqlStringType)
        {
            SqlCommand result = new SqlCommand(sqlString);

            if (filter == null)
            {
                return result;
            }

            //查询字段处理
            if(filter.QueryFields!=null && filter.QueryFields.Count > 0)
            {
                string queryFieldContactString = string.Join(',', filter.QueryFields);

                result.Sql = result.Sql.Replace("*", queryFieldContactString);
            }

            //查询条件的处理
            if (filter.Conditons != null && filter.Conditons.Count > 0)
            {
                result = GetConditionSqlCommand(result.Sql, filter.Conditons);
            }

            return result;
        }




        /// <summary>
        /// 获取查询的sqlcommand
        /// </summary>
        /// <param name="baseSqlString">基础sql</param>
        /// <param name="filterConditions"></param>
        /// <returns></returns>
        private SqlCommand GetConditionSqlCommand(string baseSqlString , List<FilterCondition> filterConditions)
        {
            var parameters = new Dictionary<string, object>();

            List<string> conditionStringList = new List<string>();

            for (int i=0; i < filterConditions.Count; i++)
            {
                string parameterName = $"@Parameter{i}";

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

            if (conditionStringList.Count > 0)
            {
                //原始的查询条件是否包括where
                bool isContainerWhere = baseSqlString.ToLower().IndexOf("where") > 0;

                //如果不含where
                if (!isContainerWhere)
                {
                    var firstConditionString = conditionStringList[0].Replace("AND", "").Replace(" OR ","");

                    conditionStringList[0] = firstConditionString;

                    baseSqlString += $" WHERE {string.Join(" ", conditionStringList)}";
                }
                else
                {
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
        private string InitSqlString<T>(SqlStringType sqlStringType)
        {
            string result = string.Empty;

            Type type = typeof(T);

            var tableNamePoperty = type.GetProperty("TableName");

            //如果没有这个属性，就不进行处理
            if (tableNamePoperty == null)
            {
                throw new Exception("model 不存在TableName的属性");
            }

            string tableName = tableNamePoperty.GetValue(Activator.CreateInstance<T>()).ToString();

            string selectSqlString = $"select * from {tableName}";

            
            switch (sqlStringType)
            {
                case SqlStringType.Select:
                    result = selectSqlString;
                    break;
            }


            Dictionary<SqlStringType, string> tempSqlStringDic = new Dictionary<SqlStringType, string>();

            tempSqlStringDic.Add(SqlStringType.Select, selectSqlString);

            _sqlStringCache.Add(type.FullName, tempSqlStringDic);


            return result;

        }
         

    }
}
