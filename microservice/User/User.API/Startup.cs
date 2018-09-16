using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.API.Middleware;
using User.Model.Logger;

namespace User.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            RegisterLogger(services);
        }

        private void RegisterLogger(IServiceCollection services)
        {
            services.Configure<LogOptions>(Configuration.GetSection("Logging"));

            services.AddSingleton<ICustomLogger, DefaultLogger>(sp =>
            {
                var logOptions = sp.GetService<IOptions<LogOptions>>();

                string host = string.Empty;
                string port = string.Empty;
                string scheme = string.Empty;


                if (logOptions != null)
                {
                    host = logOptions.Value.Host;
                    port = logOptions.Value.Port;
                    scheme = logOptions.Value.Scheme;
                }

                return new DefaultLogger(host, port, scheme);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var logger = app.ApplicationServices.GetRequiredService<ICustomLogger>();

            app.UseMiddleware<LoggerMiddleware>(logger);

            app.UseMvc();

            
        }
    }
}
