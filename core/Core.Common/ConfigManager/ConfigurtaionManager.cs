using Microsoft.Extensions.Configuration;
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
    }
}
