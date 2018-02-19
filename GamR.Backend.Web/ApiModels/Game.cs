using System.Collections.Generic;

namespace GamR.Backend.Web.ApiModels
{
    public class Game
    {
        public List<string> MeldingPlayers { get; set; }
        public string Melding { get; set; }
        public int NumberOfTricks { get; set; }
        public string NumberOfVips { get; set; }
        public List<TeamTricks> TeamTricks { get; set; }

        public List<PlayerScore> MeldingTeam { get; set; }

        public List<PlayerScore> Players { get; set; }
    }
}