using System;

namespace GamR.Backend.Core.Framework
{
    public abstract class AggregateRoot : IAggregate
    {
        public abstract Guid Id { get; }
        public long Version { get; private set; }
        
    }
}