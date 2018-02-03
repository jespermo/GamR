using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework.Impl
{
    public class InMemoryBus : IEventSubscriber, IEventPublisher
    {
        private class SubscriberInfo
        {
            public object Context { get; }
            public MethodInfo MethodInfo { get; }

            public SubscriberInfo(object context, MethodInfo methodInfo)
            {
                Context = context;
                MethodInfo = methodInfo;
            }
        }

        readonly Dictionary<Type, List<SubscriberInfo>> _subscribers = new Dictionary<Type, List<SubscriberInfo>>();
        private static readonly string HandleMethodName = nameof(ISubscribeToEvent<IEvent>.Handle);

        public Task Subscribe<T>(ISubscribeToEvent<T> subscriber) where T : IEvent
        {
            var eventType = typeof(T);
            if (!_subscribers.TryGetValue(eventType, out var existingSubscribers))
            {
                existingSubscribers = new List<SubscriberInfo>();
                _subscribers.Add(eventType, existingSubscribers);
            }
            
            existingSubscribers.Add(new SubscriberInfo(subscriber, subscriber.GetType().GetMethod(HandleMethodName, new[] { typeof(T) })));
            return Task.CompletedTask;

        }

        public async Task Publish<T>(T @event) where T : IEvent
        {
            var eventType = @event.GetType();
            if (_subscribers.TryGetValue(eventType, out var subscribers))
            {
                foreach (var subscriberInfo in subscribers)
                {
                    await (Task)subscriberInfo.MethodInfo.Invoke(subscriberInfo.Context, new object[] { @event });
                }
            }
        }
    }
}