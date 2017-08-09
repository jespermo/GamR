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
        public int NumberOfTricks { get; }
        public string NumberOfVips { get; }

        public Melded(Guid id, Guid gameId, string melding, IEnumerable<Guid> meldingPlayerIds, int numberOfTricks, string numberOfVips)
        {
            Id = id;
            GameId = gameId;
            Melding = melding;
            MeldingPlayerIds = meldingPlayerIds.ToList();
            NumberOfTricks = numberOfTricks;
            NumberOfVips = numberOfVips;
        }
    }
}