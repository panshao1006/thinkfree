using System;
using System.Collections.Generic;
using System.Text;

namespace User.Model.Filter
{
    public class BaseFilter
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
    }
}
