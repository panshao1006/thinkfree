using Core.ORM.Dapper.Enum;
using Core.ORM.Dapper.Filter;
using News.DAL;
using News.Interface.BLL;
using News.Interface.DAL;
using News.Model;
using News.Model.Filter;
using News.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace News.BLL
{
    public class NewsBusiness : BaseBusiness<NewsFilter> , INewsBusiness
    {
        private INewsRepository _dal = new NewsRepository();

       
        /// <summary>
        /// 查找新闻列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<NewsModel> GetNews(NewsFilter filter)
        {
            var dapperFilter = ConvertFilter(filter);

            List<NewsModel> newsList = _dal.GetNews(dapperFilter);

            return newsList;
        }

        /// <summary>
        /// 获取一条特定新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NewsModel GetNewsById(string id)
        {
            NewsModel result = null;

            FilterBase filter = new FilterBase();

            filter.Equal("Id", id, OperatorType.AND);


            List<NewsModel> newsList = _dal.GetNews(filter);

            if (newsList != null)
            {
                result = newsList.First();
            }

            return result;
        }


        /// <summary>
        /// 新增一条新闻
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public OperationResult AddNews(NewsModel news)
        {
           OperationResult result =  _dal.CreateNews(news);

            return result;
        }


        /// <summary>
        /// 删除一条新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OperationResult Delete(string id)
        {
            NewsModel news = new NewsModel() { Id = id };

            OperationResult result = new OperationResult();

            result.Success = _dal.Delete(news);

            if (!result.Success)
            {
                result.Messages.Add("删除失败：id为" + id);
            }

            return result;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public OperationResult Update(NewsModel news)
        {
            OperationResult result = new OperationResult();

            result.Success = _dal.Update(news);

            if (!result.Success)
            {
                result.Messages.Add("更新新闻失败：id为" + news.Id);
            }

            return result;
        }


        /// <summary>
        /// 获取新闻摘要列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<NewsAbstract> GetAbstracts(NewsFilter filter)
        {
            var dapperFilter = ConvertFilter(filter);

            dapperFilter.QueryFields = new List<string>() { "Id ", "Title", "AuthorId", "Abstract", "Tag" , "CreateDate" };

            List<NewsModel> newsAbstracts = _dal.GetNews(dapperFilter);

            var result = new List<NewsAbstract>();

            if (newsAbstracts != null)
            {
                foreach (NewsModel news in newsAbstracts)
                {
                    NewsAbstract newsAbstract = new NewsAbstract()
                    {
                        Id = news.Id,
                        Title = news.Title,
                        AuthorId = news.AuthorId,
                        Abstract = news.Abstract,
                        CreateDate = news.CreateDate
                    };
                    result.Add(newsAbstract);
                }
            }
            return result;
        }
    }
}
