using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using GamR.Backend.Core.Framework.Exceptions;

namespace GamR.Backend.Core.Framework.Impl
{
    public class InMemoryEventStore : IEventStore
    {
        class EventDescriptor
        {
            internal Guid AggregateId { get; }
            internal int Version { get; }
            internal IEvent Data { get; }

            public EventDescriptor(Guid aggregateId, IEvent data, int version)
            {
                AggregateId = aggregateId;
                Data = data;
                Version = version;
            }
        }

        readonly Dictionary<Guid, LinkedList<EventDescriptor>> _internalStore = new Dictionary<Guid, LinkedList<EventDescriptor>>();
        private readonly IEventPublisher _eventPublisher;

        public InMemoryEventStore(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task Save(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            if (!_internalStore.TryGetValue(aggregateId, out LinkedList<EventDescriptor> existingEvents))
            {
                existingEvents = new LinkedList<EventDescriptor>();
                _internalStore.Add(aggregateId, existingEvents);
            }

            var currentVersion = existingEvents.Last.Value.Version;

            if (currentVersion != expectedVersion)
                throw CreateConcurrencyException(aggregateId, events, expectedVersion, currentVersion);

            foreach (var @event in events)
            {
                var nextVersion = ++currentVersion;
                @event.Version = nextVersion;
                existingEvents.AddLast(new EventDescriptor(aggregateId, @event, nextVersion));
                await _eventPublisher.Publish(@event);
            }
        }

        public Task<IEnumerable<IEvent>> GetEventsByAggregateId(Guid aggregateId)
        {
            if (!_internalStore.TryGetValue(aggregateId, out LinkedList<EventDescriptor> eventDescriptors))
            {
                throw new AggregateNotFoundException("Could not load any events for aggregate with id '{0}'", aggregateId);
            }

            return Task.FromResult(eventDescriptors.Select(desc => desc.Data).ToImmutableList().AsEnumerable());
        }

        private ConcurrencyException CreateConcurrencyException(Guid aggregateId, IEnumerable<IEvent> events, int expectedVersion, int currentVersion)
        {
            return new ConcurrencyException(@"Could not save events, expected version {0}, but found {1} on aggregate {2}
Events (top 10):", expectedVersion, currentVersion, aggregateId, string.Join(Environment.NewLine, events.Select(e => $"{e.GetType().FullName}")));
        }
    }
}