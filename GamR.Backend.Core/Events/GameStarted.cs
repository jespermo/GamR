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
        public Guid MatchId { get; }

        public GameStarted(Guid id, Guid matchId, IEnumerable<Guid> players)
        {
            Id = id;
            MatchId = matchId;
            Players = players.ToList();
        }
        
    }
}