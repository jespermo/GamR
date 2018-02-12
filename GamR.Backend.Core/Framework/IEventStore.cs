using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework
{
    public interface IEventStore
    {
        Task Save<TEvent>(Guid aggregateId, IEnumerable<TEvent> events, long expectedVersion) where TEvent : IEvent;
        Task<IEnumerable<IEvent>> GetEventsByAggregateId(Guid aggregateId);
        Task Save(Guid aggregateId, IEnumerable<IEvent> events, long expectedVersion);
    }
}