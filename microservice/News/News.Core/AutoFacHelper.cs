using Autofac;
using News.BLL;
using News.DAL;
using News.Interface.DAL;
using System;

namespace News.Core
{
    /// <summary>
    /// autofac帮助类
    /// </summary>
    public class AutoFacHelper
    {
        public static AutoFacHelper _instance;

        private ContainerBuilder _builder;

        private IContainer _container;

        private AutoFacHelper()
        {
            _builder = new ContainerBuilder();

            _builder.RegisterType<NewsBusiness>();

            _builder.RegisterType<NewsRepository>().As<INewsRepository>();

            _container = _builder.Build();
        }

        public static AutoFacHelper Instance()
        {
            if(_instance == null)
            {
                _instance = new AutoFacHelper();
            }

            return _instance;
        }


        /// <summary>
        /// 获取容器
        /// </summary>
        /// <returns></returns>
        public IContainer GetContainer()
        {
            return _container;
        }


    }
}
