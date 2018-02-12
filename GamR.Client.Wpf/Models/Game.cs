using System;
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
        public List<PlayerScore> Players { get; set; }

        public string PlayerScores => string.Join(" | ", Players.Select(p => $"{p.Name}: {p.Score}"));

        public List<PlayerScore> MeldingTeam { get; set; }

        public List<TeamTricks> TeamTricks { get; set; }
        public string ActualNumberOfTricks => string.Join(",", TeamTricks.Select(tt => $"{Players.Single(p => p.Id == tt.TeamId).Name}: {tt.Result}"));
    }

    public class TeamTricks
    {
        public Guid TeamId { get; set; }
        public int Result { get; set; }
    }
}