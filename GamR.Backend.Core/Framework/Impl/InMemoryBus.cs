using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework.Impl
{

    public interface ISubscribeToEvent<T> where T : IEvent
    {
        Task Handle(T args);
    }
    

    public class InMemoryBus : IEventSubscriber, IEventPublisher
    {
        readonly Dictionary<Type, List<dynamic>>_subscribers = new Dictionary<Type, List<dynamic>>();

        public Task Subscribe<T>(ISubscribeToEvent<T> subscriber) where T: IEvent
        {
            var eventType = typeof(T);
            if (!_subscribers.TryGetValue(eventType, out var existingSubscribers))
            {
                existingSubscribers = new List<dynamic>();
                _subscribers.Add(eventType, existingSubscribers);
            }
            existingSubscribers.Add(subscriber);
            return Task.CompletedTask;

        }

        public async Task Publish<T>(T @event) where T: IEvent
        {
            var eventType = @event.GetType();
            if (_subscribers.TryGetValue(eventType, out var subscribers))
            {
                //                                      :-)
                foreach (dynamic subscriber in subscribers)
                {
                    await subscriber.Handle((dynamic)@event);
                }
            }
        }
    }
}