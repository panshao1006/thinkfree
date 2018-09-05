using News.Model;
using News.Model.Filter;
using News.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.Interface.BLL
{
    public interface INewsBusiness
    {
        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<NewsModel> GetNews(NewsFilter filter);


        /// <summary>
        /// 获取新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NewsModel GetNewsById(string id);


        /// <summary>
        /// 新增一条新闻
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        OperationResult AddNews(NewsModel news);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperationResult Delete(string id);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        OperationResult Update(NewsModel news);


        /// <summary>
        /// 获取新闻摘要列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<NewsAbstract> GetAbstracts(NewsFilter filter);
    }
}
