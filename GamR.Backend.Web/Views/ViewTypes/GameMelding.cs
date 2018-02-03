using System;
using System.Collections.Generic;

namespace GamR.Backend.Web.Views.ViewTypes
{
    public class GameMelding
    {
        private readonly string _numberOfVips;
        public string Melding { get; }
        public IEnumerable<Guid> MeldingPlayers { get; }
        public int NumberOfTricks { get; }

        public GameMelding(string melding, IEnumerable<Guid> meldingPlayers, int numberOfTricks, string numberOfVips)
        {
            _numberOfVips = numberOfVips;
            Melding = melding;
            MeldingPlayers = meldingPlayers;
            NumberOfTricks = numberOfTricks;
        }
    }
}