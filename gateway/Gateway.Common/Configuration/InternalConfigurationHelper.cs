using Gateway.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gateway.Common.Configuration
{
    /// <summary>
    /// 配置帮助类 单例
    /// </summary>
    public class InternalConfigurationHelper
    {
        private static InternalConfigurationHelper _instance;

        private FileConfiguration _fileConfiguration;

        private InternalConfigurationHelper(FileConfiguration fileConfiguration)
        {
            _fileConfiguration = fileConfiguration;
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="fileConfiguration"></param>
        /// <returns></returns>
        public static InternalConfigurationHelper Instance(FileConfiguration fileConfiguration = null)
        {
            if(_instance == null)
            {
                _instance = new InternalConfigurationHelper(fileConfiguration);
            }

            return _instance;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public FileConfiguration GetConfiguration()
        {
            return _fileConfiguration;
        }
    }
}
