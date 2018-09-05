using Core.ORM.Dapper.Filter;
using News.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.BLL
{
    public class BaseBusiness<T> where T : BaseFilter
    {
        /// <summary>
        /// 过滤条件的转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userFilter"></param>
        /// <returns></returns>
        protected FilterBase ConvertFilter(T filter)
        {
            FilterBase dapperFilter = new FilterBase();

            if(filter == null)
            {
                return dapperFilter;
            }

            dapperFilter.PageIndex = filter.PageIndex;

            dapperFilter.PageSize = filter.PageSize;

            dapperFilter.SortField = filter.SortField;

            dapperFilter.Sort = filter.Sort;

            dapperFilter.QueryFields = filter.QueryFields;

            CustomConvert(dapperFilter, filter);

            return dapperFilter;
        }


        /// <summary>
        /// 自定义的转换
        /// </summary>
        /// <param name="dapperFilter"></param>
        /// <param name="filter"></param>
        protected virtual void CustomConvert(FilterBase dapperFilter, T filter)
        {

        }
    }
}
