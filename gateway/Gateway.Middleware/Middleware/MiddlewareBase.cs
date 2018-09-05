using Gateway.Model;
using Gateway.Model.CustomError;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Middleware.Middleware
{
    public abstract class MiddlewareBase
    {
        protected MiddlewareBase()
        {
            //Logger = logger;
            MiddlewareName = this.GetType().Name;
        }

        public string MiddlewareName { get; }

        public void SetPipelineError(DownstreamContext context, List<ErrorBase> errors)
        {
            foreach (var error in errors)
            {
                SetPipelineError(context, error);
            }
        }

        public void SetPipelineError(DownstreamContext context, ErrorBase error)
        {
            context.Errors.Add(error);
        }
    }
}
