﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using GamR.Backend.Core.Framework.Exceptions;

namespace GamR.Backend.Core.Framework.Impl
{
    public class EventDescriptor
    {
        public Guid AggregateId { get; private set; }
        public long Version { get; private set; }
        public IEvent Data { get; private set; }

        public EventDescriptor(Guid aggregateId, IEvent data, long version)
        {
            AggregateId = aggregateId;
            Data = data;
            Version = version;
        }
    }

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

        public Task Save<TEvent>(Guid aggregateId, IEnumerable<TEvent> events, long expectedVersion) where TEvent : IEvent
        {
            return Save(aggregateId, events.Cast<IEvent>(), expectedVersion);
        }

        public async Task Save(Guid aggregateId, IEnumerable<IEvent> events, long expectedVersion)
        {
            if (!_internalStore.TryGetValue(aggregateId, out LinkedList<EventDescriptor> existingEvents))
            {
                existingEvents = new LinkedList<EventDescriptor>();
                _internalStore.Add(aggregateId, existingEvents);
            }
            
            var currentVersion = existingEvents?.Last?.Value?.Version ?? 0;

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

        public async Task<Dictionary<Guid,IEnumerable<IEvent>>> GetEventsByAggregateType(Type aggregateType)
        {
            return await Task.FromResult(_internalStore.Values
                .SelectMany(x=>x)
                .Where(x=>x.Data.AggregateType == aggregateType)
                .OrderBy(x=>x.Version)
                .GroupBy(x=>x.AggregateId)
                .ToDictionary(x => x.Key, y => y.Select(x=>x.Data).ToImmutableList().AsEnumerable()));
        }

        private ConcurrencyException CreateConcurrencyException(Guid aggregateId, IEnumerable<IEvent> events, long expectedVersion, long currentVersion)
        {
            return new ConcurrencyException(@"Could not save events, expected version {0}, but found {1} on aggregate {2}
Events (top 10):", expectedVersion, currentVersion, aggregateId, string.Join(Environment.NewLine, events.Take(10).Select(e => $"{e.GetType().FullName}")));
        }
    }
}