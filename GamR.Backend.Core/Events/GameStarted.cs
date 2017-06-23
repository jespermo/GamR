using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Events
{
    public class GameStarted : Event
    {
        public Guid Id { get; }
        public IReadOnlyCollection<Guid> Players { get; }

        public GameStarted(Guid id, IEnumerable<Guid> players)
        {
            Id = id;
            Players = players.ToList();
        }
        
    }
}