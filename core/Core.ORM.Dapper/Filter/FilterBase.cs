using Core.ORM.Dapper.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ORM.Dapper.Filter
{
    /// <summary>
    /// 过滤基类
    /// </summary>
    public class FilterBase
    {
        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { set; get; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { set; get; }

        /// <summary>
        /// 页数
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 获取字段
        /// </summary>
        public List<string> QueryFields { set; get; }

        /// <summary>
        /// 条件
        /// </summary>
        public List<FilterCondition> Conditons { set; get; }

        /// <summary>
        /// 自定义的查询条件语句块
        /// </summary>
        public List<FilterSegment> FilterSegments { set; get; }

        public FilterBase()
        {
            Conditons = new List<FilterCondition>();

            FilterSegments = new List<FilterSegment>();
        }

        public void Equal(string field , string value , OperatorType operatorType = OperatorType.AND)
        {
            FilterCondition condition = new FilterCondition();

            condition.Field = field;
            condition.LogicType = LogicType.Equal;
            condition.Value = value;

            Conditons.Add(condition);
        }

        /// <summary>
        /// 增加查询块，例如 { a= 1 and b=2}
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="operatorType"></param>
        public void AddFilterSegment(List<FilterCondition> conditions , OperatorType operatorType = OperatorType.AND)
        {
            if(conditions == null)
            {
                throw new ArgumentNullException("conditions 不能为null");
            }

            FilterSegment filterSegment = new FilterSegment();
            filterSegment.Conditions = conditions;
            filterSegment.OperatorType = operatorType;

            FilterSegments.Add(filterSegment);
        }
    }
}
