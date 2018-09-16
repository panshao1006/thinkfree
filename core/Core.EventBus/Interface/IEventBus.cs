using Core.EventBus.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.EventBus.Interface
{
    public interface IEventBus
    {
        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <param name="handler"></param>
        void Subscribe<T, TH>(Func<TH> handler)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

        void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="event"></param>
        void Publish(IntegrationEvent @event);
    }
}
