using Gateway.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Common
{
    public class HttpResponser
    {
        //private DownstreamContext _context;

        //private readonly IRemoveOutputHeaders _removeOutputHeaders;

        public HttpResponser()
        {
            //_context = context;
        }


        public async Task SetResponseOnHttpContext(HttpContext context, DownstreamResponse response)
        {
            //_removeOutputHeaders.Remove(response.Headers);

            //foreach (var httpResponseHeader in response.Headers)
            //{
            //    AddHeaderIfDoesntExist(context, httpResponseHeader);
            //}

            foreach (var httpResponseHeader in response.Content.Headers)
            {
                AddHeaderIfDoesntExist(context, new Header(httpResponseHeader.Key, httpResponseHeader.Value));
            }

            var content = await response.Content.ReadAsStreamAsync();

            AddHeaderIfDoesntExist(context, new Header("Content-Length", new[] { content.Length.ToString() }));

            context.Response.OnStarting(state =>
            {
                var httpContext = (HttpContext)state;

                httpContext.Response.StatusCode = (int)response.StatusCode;

                return Task.CompletedTask;
            }, context);

            using (content)
            {
                if (response.StatusCode != HttpStatusCode.NotModified && context.Response.ContentLength != 0)
                {
                    await content.CopyToAsync(context.Response.Body);
                }
            }
        }


        public void SetErrorResponseOnContext(HttpContext context, int statusCode)
        {
            context.Response.StatusCode = statusCode;
        }

        private static void AddHeaderIfDoesntExist(HttpContext context, Header httpResponseHeader)
        {
            if (!context.Response.Headers.ContainsKey(httpResponseHeader.Key))
            {
                context.Response.Headers.Add(httpResponseHeader.Key, new StringValues(httpResponseHeader.Values.ToArray()));
            }
        }



    }
}
