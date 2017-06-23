using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework
{
    public interface IEventStore
    {
        Task Save(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
    }
}