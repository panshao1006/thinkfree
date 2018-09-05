using System;
using System.Collections.Generic;
using System.Text;

namespace News.Model.ViewModel
{
    public class NewsAbstract
    {
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
        public string AuthorName { set; get; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { set; get; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { set; get; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { set; get; }
    }
}
