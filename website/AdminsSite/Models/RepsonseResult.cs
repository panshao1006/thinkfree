using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminsSite.Models
{
    /// <summary>
    /// 请求结果返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepsonseResult<T>
    {
        public RepsonseResult()
        {
            Errors = new List<string>();
        }

        public RepsonseResult(T data) : base()
        {
            Data = data;
        }

        public List<string> Errors { set; get; }

        public T Data { set; get; }

        [JsonIgnore]
        public bool IsError => Errors.Count > 0;

        public bool IsSuccess { set; get; }
    }
}
