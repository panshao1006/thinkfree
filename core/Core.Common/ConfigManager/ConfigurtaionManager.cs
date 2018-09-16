using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

namespace Core.Common.ConfigManager
{
    public class ConfigurtaionManager
    {
        public static IConfiguration _configuration { get; set; }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AppSettings(string key)
        {
            if (_configuration == null)
            {
                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");

                _configuration = builder.Build();
            }

            return _configuration[key];
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T AppSettings<T>(string key) where T : class, new()
        {
            if (_configuration == null)
            {
                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");

                _configuration = builder.Build();
            }


            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(_configuration.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }
    }
}
