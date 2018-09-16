using System;
using Core.Common.ConfigManager;
using Core.Log;
using Core.ORM.Dapper;
using News.Model.Logger;

namespace News.DAL
{
    /// <summary>
    /// 数据访问层基类
    /// </summary>
    public class RepositoryBase
    {
        protected DapperExtension _dal = new DapperExtension();

        protected ICustomLogger _logger;

        protected LogOptions _logOptions;

        public RepositoryBase()
        {
            _dal.ExecutedEvent += ExecutedEvent;

            _logOptions = ConfigurtaionManager.AppSettings<LogOptions>("logging");

            _logger = new DefaultLogger(_logOptions.Host , _logOptions.Port , _logOptions.Scheme);
        }

        private void ExecutedEvent(string sqlcommand)
        {
            _logger.Info<BaseLogModel>(GetLogModel(sqlcommand));
        }


        protected BaseLogModel GetLogModel(string sqlcommand)
        {
            BaseLogModel baseLogModel = new BaseLogModel()
            {
                Type = 5,
                ServiceName = _logOptions.ServiceName,
                Content = sqlcommand
            };

            return baseLogModel;
        }
    }
}
