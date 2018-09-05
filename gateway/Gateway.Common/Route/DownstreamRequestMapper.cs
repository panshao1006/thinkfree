using Gateway.Model;
using Gateway.Model.CustomError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Common.Route
{
    /// <summary>
    /// 请求创建类
    /// </summary>
    public class DownstreamRequestMapper
    {
        private HttpRequest _request;

        private List<ErrorBase> _errors;

        public bool IsError => _errors.Count > 0;

        /// <summary>
        /// 不支持的httpreqeustheader
        /// </summary>
        private readonly string[] _unsupportedHeaders = { "host" };

        public DownstreamRequestMapper(HttpRequest request)
        {
            _request = request;
            _errors = new List<ErrorBase>();
        }

        public DownstreamRequest Create()
        {
            var httpRequestMessage = Map().Result;

            DownstreamRequest downstreamRequest = new DownstreamRequest(httpRequestMessage);

            return downstreamRequest;
        }

        public async Task<HttpRequestMessage> Map()
        {
            try
            {
                HttpRequest request = _request;

                var requestMessage = new HttpRequestMessage()
                {
                    Content = await MapContent(request),
                    Method = MapMethod(request),
                    RequestUri = MapUri(request)
                };

                MapHeaders(request, requestMessage);

                return requestMessage;
            }
            catch (Exception ex)
            {
                var error = new UnmappableRequestError(ex);

                _errors.Add(error);

                return null;
            }
        }

        private async Task<HttpContent> MapContent(HttpRequest request)
        {
            if (request.Body == null || (request.Body.CanSeek && request.Body.Length <= 0))
            {
                return null;
            }

            var content = new ByteArrayContent(await ToByteArray(request.Body));

            if (!string.IsNullOrEmpty(request.ContentType))
            {
                content.Headers
                    .TryAddWithoutValidation("Content-Type", new[] { request.ContentType });
            }

            AddHeaderIfExistsOnRequest("Content-Language", content, request);
            AddHeaderIfExistsOnRequest("Content-Location", content, request);
            AddHeaderIfExistsOnRequest("Content-Range", content, request);
            AddHeaderIfExistsOnRequest("Content-MD5", content, request);
            AddHeaderIfExistsOnRequest("Content-Disposition", content, request);
            AddHeaderIfExistsOnRequest("Content-Encoding", content, request);

            return content;
        }

        private async Task<byte[]> ToByteArray(Stream stream)
        {
            using (stream)
            {
                using (var memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    return memStream.ToArray();
                }
            }
        }


        private void AddHeaderIfExistsOnRequest(string key, HttpContent content, HttpRequest request)
        {
            if (request.Headers.ContainsKey(key))
            {
                content.Headers
                    .TryAddWithoutValidation(key, request.Headers[key].ToList());
            }
        }


        private void MapHeaders(HttpRequest request, HttpRequestMessage requestMessage)
        {
            foreach (var header in request.Headers)
            {
                if (IsSupportedHeader(header))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
        }


        private HttpMethod MapMethod(HttpRequest request)
        {
            return new HttpMethod(request.Method);
        }


        private Uri MapUri(HttpRequest request)
        {
            return new Uri(request.GetEncodedUrl());
        }

        private bool IsSupportedHeader(KeyValuePair<string, StringValues> header)
        {
            return !_unsupportedHeaders.Contains(header.Key.ToLower());
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns></returns>
        public List<ErrorBase> GetError()
        {
            return _errors;
        }
    }
}
