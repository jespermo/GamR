using System;
using System.Threading.Tasks;
using GamR.Backend.Core.Framework.Impl;

namespace GamR.Backend.Core.Framework
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }

    public interface IEventSubscriber
    {
        Task Subscribe<T>(ISubscribeToEvent<T> handler) where T : IEvent;
    }
}