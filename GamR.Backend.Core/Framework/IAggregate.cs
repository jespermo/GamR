using System;
using System.Collections.Generic;

namespace GamR.Backend.Core.Framework
{
    public interface IAggregate
    {
        Guid Id { get; }
        long Version { get; }

        void Hydrate<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent;
        IEnumerable<Event> UncommittedChanges();
        void MarkUncommittedChangesAsCommitted();
    }
}