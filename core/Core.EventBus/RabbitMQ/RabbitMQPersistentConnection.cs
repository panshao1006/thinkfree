using Core.Common.ConfigManager;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection
    {
        private IConnectionFactory _factory;

        private IConnection _connection;

        public RabbitMQPersistentConnection()
        {
            _factory = GetConnectionFactory();
        }


        private IConnectionFactory GetConnectionFactory()
        {
           var  factory = new ConnectionFactory();
            //RabbitMQ服务在本地运行
            factory.HostName = ConfigurtaionManager.AppSettings("RabbitHost");
            //用户名
            factory.UserName = ConfigurtaionManager.AppSettings("RabbitUserName");
            //密码
            factory.Password = ConfigurtaionManager.AppSettings("RabbitPassword");

            return factory;
        }


        public bool TryConnect()
        {
            _connection = _factory.CreateConnection();

            if (IsConnected)
            {
                //_connection.ConnectionShutdown += OnConnectionShutdown;
                //_connection.CallbackException += OnCallbackException;
                //_connection.ConnectionBlocked += OnConnectionBlocked;

                return true;
            }
            else
            {

                return false;
            }
        }


        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen;
            }
        }


        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }


    }
}
