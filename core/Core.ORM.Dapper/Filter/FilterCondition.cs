using Core.ORM.Dapper.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ORM.Dapper.Filter
{
    public class FilterCondition
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Field { set; get; }

        /// <summary>
        /// 逻辑，IN , = ,<>
        /// </summary>
        public LogicType LogicType { set; get; }

        /// <summary>
        /// sql运算符
        /// </summary>
        public OperatorType OperatorType { set; get; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { set; get; }

        public FilterCondition()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="logicType">操作符 in = <>等</param>
        /// <param name="operatorType">运算符 and or</param>
        /// <param name="value">比较值</param>
        public FilterCondition(string field , LogicType logicType , OperatorType operatorType , object value)
        {
            Field = field;
            LogicType = logicType;
            OperatorType = operatorType;
            Value = value;
        }
    }
}
