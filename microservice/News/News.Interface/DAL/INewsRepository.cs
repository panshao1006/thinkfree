using Core.ORM.Dapper.Filter;
using News.Model;
using News.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.Interface.DAL
{
    public interface INewsRepository
    {
        /// <summary>
        /// 新增新闻
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        OperationResult CreateNews(NewsModel news);

        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<NewsModel> GetNews(FilterBase filter);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        bool Delete(NewsModel news);


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        bool Update(NewsModel news);


    }
}
