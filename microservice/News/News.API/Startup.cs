using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.EventBus.Interface;
using Core.EventBus.RabbitMQ;
using Core.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using News.API.Middleware;
using News.Model.Logger;

namespace News.API
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

            //RegisterEventBus(services);

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

                return new DefaultLogger(host ,port ,scheme);
            });
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            //订阅队列名称
            var subscriptionClientName = Configuration["SubscriptionClientName"].ToString();

            services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
            {
                //var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                //var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                //var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                //var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                //var retryCount = 5;
                //if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                //{
                //    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                //}

                return new RabbitMQEventBus(subscriptionClientName);
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

            app.UseMiddleware<ErrorHandlingMiddleware>(logger);
            app.UseMiddleware<LoggerMiddleware>(logger);

            app.UseMvc();

            
        }
    }
}
