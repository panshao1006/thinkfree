using Core.ORM.Dapper.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ORM.Dapper.Filter
{
    public class FilterSegment
    {
        /// <summary>
        /// 块的减值对
        /// </summary>
        public List<FilterCondition> Conditions { set; get; }

        /// <summary>
        /// KeyValues的内部运算符
        /// </summary>
        public OperatorType InnerOperatorType { set; get; }

        /// <summary>
        /// 块与块之间的运算符
        /// </summary>
        public OperatorType OperatorType { set; get; }

        public FilterSegment()
        {
            Conditions = new List<FilterCondition>();
        }
    }
}
