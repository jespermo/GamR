using System;

namespace GamR.Backend.Core.Framework
{
    public interface IAggregate
    {
        Guid Id { get; }
        long Version { get; }
    }
}