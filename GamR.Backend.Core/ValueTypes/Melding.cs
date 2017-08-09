using System;
using System.Collections.Generic;

namespace GamR.Backend.Core.ValueTypes
{
    public class Melding
    {
        public string Type { get; }

        public IEnumerable<Guid> MeldingPlayerIds { get; }
        public int NumberOfTricks { get; }
        public string NumberOfVips { get; }

        public Melding(string melding, IEnumerable<Guid> meldingPlayerIds, int numberOfTricks, string numberOfVips)
        {
            Type = melding;
            MeldingPlayerIds = meldingPlayerIds;
            NumberOfTricks = numberOfTricks;
            NumberOfVips = numberOfVips;
        }
    }
}