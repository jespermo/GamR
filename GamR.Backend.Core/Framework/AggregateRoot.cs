using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GamR.Backend.Core.Framework
{
    public abstract class AggregateRoot : IAggregate
    {
        private readonly List<IEvent> _uncommitted = new List<IEvent>();
        public abstract Guid Id { get; }
        public long Version { get; private set; }

        public void Hydrate<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                Apply(@event, false);
            }
        }

        public IEnumerable<Event> UncommittedChanges()
        {
            return _uncommitted.Cast<Event>().ToList();
        }
        public void MarkUncommittedChangesAsCommitted()
        {
            _uncommitted.Clear();
        }

        public void Apply2<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Apply(@event, true);
        }

        private void Apply<T>(T @event, bool isNew) where T : IEvent
        {
            try
            {
                ((dynamic) this).Apply((dynamic)@event);

                if (isNew)
                    _uncommitted.Add(@event);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}