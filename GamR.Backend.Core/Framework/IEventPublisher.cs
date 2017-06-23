using System;
using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }

    public interface IEventSubscriber
    {
        Task Subscribe<T>(Func<T, Task> handler) where T : IEvent;
    }
}