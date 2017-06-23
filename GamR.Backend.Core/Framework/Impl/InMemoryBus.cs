using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework.Impl
{
    public class InMemoryBus : IEventSubscriber, IEventPublisher
    {
        // adddd find noget bedre end List<object> 
        readonly Dictionary<Type, List<object>>_subscribers = new Dictionary<Type, List<object>>();

        public Task Subscribe<T>(Func<T, Task> handler) where T: IEvent
        {
            var eventType = typeof(T);
            if (!_subscribers.TryGetValue(eventType, out var existingSubscribers))
            {
                existingSubscribers = new List<object>();
                _subscribers.Add(eventType, existingSubscribers);
            }
            existingSubscribers.Add(handler);
            return Task.CompletedTask;

        }

        public async Task Publish<T>(T @event) where T: IEvent
        {
            var eventType = typeof(T);
            if (_subscribers.TryGetValue(eventType, out var subscribers))
            {
                //                                              :-)
                foreach (var subscriber in subscribers.Cast<Func<T, Task>>())
                {
                    await subscriber(@event);
                }
            }
        }
    }
}