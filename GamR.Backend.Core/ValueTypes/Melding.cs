using System;
using System.Collections.Generic;

namespace GamR.Backend.Core.ValueTypes
{
    public class Melding
    {
        public string Type { get; }

        public IEnumerable<Guid> MeldingPlayerIds { get; }
        public IEnumerable<Guid> MeldingTeamPlayerIds { get; }
        public int NumberOfTricks { get; }
        public string NumberOfVips { get; }

        public Melding(string melding, IEnumerable<Guid> meldingPlayerIds, IEnumerable<Guid> meldingTeamPlayerIds, int numberOfTricks, string numberOfVips)
        {
            Type = melding;
            MeldingPlayerIds = meldingPlayerIds;
            MeldingTeamPlayerIds = meldingTeamPlayerIds;
            NumberOfTricks = numberOfTricks;
            NumberOfVips = numberOfVips;
        }
    }
}