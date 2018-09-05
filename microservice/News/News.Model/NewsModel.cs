using Newtonsoft.Json;
using System;

namespace News.Model
{
    public class NewsModel: BaseModel
    {
        public NewsModel() : base("thinkfree_news")
        {

        }

        /// <summary>
        /// 新闻id
        /// </summary>
        public string Id { set; get; }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }


        /// <summary>
        /// 作者ID
        /// </summary>
        public string AuthorId { set; get; }


        /// <summary>
        /// 作者名称
        /// </summary>
        [JsonIgnore]
        public string AuthorName { set; get; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { set; get; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { set; get; }


        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { set; get; }

        /// <summary>
        /// 修改日期
        /// </summary>
        [JsonIgnore]
        public DateTime ModifyDate { set; get; }
    }
}
