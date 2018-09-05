using Core.ORM.Dapper.Filter;
using News.Interface.DAL;
using News.Model;
using News.Model.ViewModel;
using System;
using System.Collections.Generic;

namespace News.DAL
{
    public class NewsRepository : RepositoryBase, INewsRepository
    {

        /// <summary>
        /// 新增一条新闻
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public OperationResult CreateNews(NewsModel news)
        {
            var result = _dal.Insert<NewsModel>(news);

            OperationResult returnResult = new OperationResult();

            if (result == null)
            {
                returnResult.Success = false;
                returnResult.Messages.Add("新闻新增失败");
            }
            else
            {
                returnResult.Success = true;
            }

            return returnResult;
        }

        /// <summary>
        /// 查找新闻列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<NewsModel> GetNews(FilterBase filter)
        {
            List<NewsModel> newsList =  _dal.Query<NewsModel>(filter);

            return newsList;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public bool Delete(NewsModel news)
        {
            return _dal.Delete<NewsModel>(news);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public bool Update(NewsModel news)
        {
            return _dal.Update<NewsModel>(news);
        }
    }
}
