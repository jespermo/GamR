using System.Collections.Generic;
using System.Linq;

namespace GamR.Client.Wpf.Models
{
    public class Game
    {
        public List<string> MeldingPlayers { get; set; }

        public string MeldingPlayersString => MeldingPlayers != null ? string.Join(",", MeldingPlayers) : Melding;

        public string Melding { get; set; }
        public int NumberOfTricks { get; set; }
        public string NumberOfVips { get; set; }
        public int ActualNumberOfTricks { get; set; }
        public List<PlayerScore> Players { get; set; }

        public string PlayerScores => string.Join(" | ", Players.Select(p => $"{p.Name}: {p.Score}"));
    }
}