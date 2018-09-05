using Core.Common;
using Core.Common.CustomException;
using Microsoft.AspNetCore.Http;
using News.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Middleware
{
    /// <summary>
    /// 错误处理中间件
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }


        /// <summary>
        /// 错误中间件委托方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is SystemException)
                {
                    statusCode = 500;
                }
                else if (ex is BusinessException)
                {
                    statusCode = (int)((BusinessException)ex).GetErrorCode();
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var message = "";

                switch (statusCode)
                {
                    case 401:
                        message = "未授权";
                        break;
                    case 404:
                        message = "未找到服务";
                        break;
                    case 502:
                        message = "请求错误";
                        break;
                    default:
                        message = "";
                        break;
                }
               
                if (!string.IsNullOrWhiteSpace(message))
                {
                    await HandleExceptionAsync(context, statusCode, message);
                }
            }
        }

        /// <summary>
        /// 返回错误格式
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            ErrorResultModel errorModel = new ErrorResultModel()
            {
                Message = msg,
                Code = statusCode,
                RequestUri = context.Request.GetAbsoluteUri()
            };

            var result = JsonConvert.SerializeObject(new { error = errorModel });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
}
