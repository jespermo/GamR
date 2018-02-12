using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Events
{
    public class Melded : Event
    {
        public Guid Id { get; }
        public Guid GameId { get; }
        public string Melding { get; }

        public IEnumerable<Guid> MeldingPlayerIds { get; }
        public IEnumerable<Guid> MeldingTeamPlayerIds { get; }
        public int NumberOfTricks { get; }
        public string NumberOfVips { get; }

        public Guid MatchId { get; }

        public Melded(Guid id, Guid gameId,  Guid matchId, string melding, IEnumerable<Guid> meldingPlayerIds, 
            IEnumerable<Guid> meldingTeamPlayerIds,
            int numberOfTricks, string numberOfVips)
        {
            Id = id;
            GameId = gameId;
            MatchId = matchId;
            Melding = melding;
            MeldingPlayerIds = meldingPlayerIds.ToList();
            MeldingTeamPlayerIds = meldingTeamPlayerIds.ToList();
            NumberOfTricks = numberOfTricks;
            NumberOfVips = numberOfVips;
        }
    }
}