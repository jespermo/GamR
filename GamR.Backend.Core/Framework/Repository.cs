using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<T>> GetAll()
        {
            var items = new List<T>();
            var eventsForAggregate = await _eventStore.GetEventsByAggregateType(typeof(T));
            foreach (var @event in eventsForAggregate)
            {
                var aggregate = Activator.CreateInstance<T>();
                aggregate.Hydrate(@event.Value);
                items.Add(aggregate);
            }
            return items;;
        }

        public async Task Save(T aggregate)
        {
            var changes = aggregate.UncommittedChanges();
            await _eventStore.Save<IEvent>(aggregate.Id, changes, aggregate.Version);
            aggregate.MarkUncommittedChangesAsCommitted();
        }
    }
}