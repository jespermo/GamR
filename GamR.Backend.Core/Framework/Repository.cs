﻿using System;
using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework
{
    public class Repository<T> : IRepository<T> where T : IAggregate
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task<T> GetById(Guid aggregateId)
        {
            var result = await _eventStore.GetEventsByAggregateId(aggregateId);
            var instance = Activator.CreateInstance<T>();

            instance.Hydrate(result);
            return instance;
        }
            
        public async Task Save(T aggregate)
        {
            var changes = aggregate.UncommittedChanges();
            await _eventStore.Save(aggregate.Id, changes, aggregate.Version);
            aggregate.MarkUncommittedChangesAsCommitted();
        }
    }
}