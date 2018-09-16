using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.EventBus.Interface;
using Core.EventBus.RabbitMQ;
using Log.BLL;
using Log.BLL.Events;
using Log.Interface.BLL;
using Log.Model.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Log.API
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

            //注册接口
            services.AddTransient<ILogBusiness, LogBusiness>();
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

            app.UseMvc();

            //ConfigureEventBus(app);
        }


        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<LogAddedIntegrationEvent, LogAddedIntegrationEventHandler>();
        }

    }
}
