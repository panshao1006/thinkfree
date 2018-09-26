using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Log
{
    public interface ICustomLogger
    {
        /// <summary>
        /// 致命错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="log"></param>
        /// <returns></returns>
        Task Fatal<T>(T log);

        /// <summary>
        /// 一般错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="log"></param>
        /// <returns></returns>
        Task Error<T>(T log);

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="log"></param>
        /// <returns></returns>
        Task Info<T>(T log);
    }
}
