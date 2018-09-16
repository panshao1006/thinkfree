using Core.Log;
using Gateway.Common.DependencyInjection;
using Gateway.Middleware;
using Gateway.Model.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Gateway.API
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("configuration.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCustomServices(Configuration);

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

            app.UseExtendMiddleware();
        }
    }
}
