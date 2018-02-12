using System;
using System.Collections.Generic;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Events
{
    public class GameEnded : Event
    {
        public Guid Id { get; }
        public Guid GameId { get; }
        public Guid MatchId { get; }
        public Dictionary<Guid, decimal> Result { get; }
        public Dictionary<Guid, int> TeamTricks { get; }


        public GameEnded(Guid id, Guid gameId, Guid matchId, Dictionary<Guid,decimal> result, Dictionary<Guid, int> teamTricks)
        {
            Id = id;
            GameId = gameId;
            MatchId = matchId;
            Result = result;
            TeamTricks = teamTricks;
        }
    }
}