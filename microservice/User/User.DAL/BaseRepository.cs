using Core.Common.ConfigManager;
using Core.Log;
using Core.ORM.Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using User.Model.Logger;

namespace User.DAL
{
    public class BaseRepository
    {
        protected DapperExtension _dal = new DapperExtension();

        protected ICustomLogger _logger;

        protected LogOptions _logOptions;

        public BaseRepository()
        {
            _dal.ExecutedEvent += ExecutedEvent;

            _logOptions = ConfigurtaionManager.AppSettings<LogOptions>("logging");

            _logger = new DefaultLogger(_logOptions.Host, _logOptions.Port, _logOptions.Scheme);
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
                Content = sqlcommand,
                ServiceName = _logOptions.ServiceName
            };

            return baseLogModel;
        }
    }
}
