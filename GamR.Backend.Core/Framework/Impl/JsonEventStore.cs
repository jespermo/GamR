using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GamR.Backend.Core.Framework.Impl
{
    public class JsonEventStore : IEventStore
    {
        private readonly IEventStore _eventStore;
        private readonly string _eventStoreFileName;

        
        
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public JsonEventStore(IEventStore eventStore, string eventStoreFileName)
        {
            _eventStore = eventStore;
            _eventStoreFileName = eventStoreFileName;

            
            Task.Run((Func<Task>) (async () =>
            {
                await StoreInteraction(async () =>
                {
                    if (!File.Exists(eventStoreFileName))
                        File.Create(eventStoreFileName).Dispose();


                    var events = File.ReadLines(eventStoreFileName).Select(x=> JsonConvert.DeserializeObject<EventDescriptor>(x, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All}));

                    foreach (var eventDescriptor in events.GroupBy(x => x.AggregateId))
                    {
                        await _eventStore.Save(eventDescriptor.Key,
                            eventDescriptor.OrderBy(x => x.Version).Select(x => x.Data),
                            eventDescriptor.Max(x => x.Version));
                    }
                });
            })).GetAwaiter().GetResult();
        }
        

        private async Task StoreInteraction(Func<Task> action)
        {
            await _semaphore.WaitAsync();
            try
            {
                await action();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public Task Save<TEvent>(Guid aggregateId, IEnumerable<TEvent> events, long expectedVersion) where TEvent : IEvent
        {
            return Save(aggregateId, events.Cast<IEvent>(), expectedVersion);
        }

        public Task<IEnumerable<IEvent>> GetEventsByAggregateId(Guid aggregateId)
        {
            return _eventStore.GetEventsByAggregateId(aggregateId);
        }

        public async Task Save(Guid aggregateId, IEnumerable<IEvent> events, long expectedVersion)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            await _eventStore.Save(aggregateId, events.ToList(), expectedVersion);
            await StoreInteraction(async () =>
            {
                var objects =
                    events.ToList().Select(x => JsonConvert.SerializeObject(new EventDescriptor(aggregateId, x, expectedVersion),
                        Formatting.None, jsonSerializerSettings));
                try
                {
                    File.AppendAllLines(_eventStoreFileName, objects);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
        }

        public Task<Dictionary<Guid, IEnumerable<IEvent>>> GetEventsByAggregateType(Type aggregateType)
        {
            return _eventStore.GetEventsByAggregateType(aggregateType);
        }
    }
}