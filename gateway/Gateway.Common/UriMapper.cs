using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Gateway.Common
{
    public class UriMapper
    {
        /// <summary>
        /// 获取restfull风格的URI
        /// </summary>
        /// <param name="path"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string GetRestfullUri(string path , string queryString)
        {
            if (string.IsNullOrWhiteSpace(queryString))
            {
                return path;
            }

            var queryArray = HttpUtility.ParseQueryString(queryString);

            //如果有多个参数，还是采用原来的?id=1&name=2的结构
            if (queryArray.Count == 1)
            {
                //如果只有一个参数，采用api/values/1的形式
                path += queryArray[0];
            }

            return path;
        }
    }
}
