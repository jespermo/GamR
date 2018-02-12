using System;
using System.Collections.Generic;

namespace GamR.Backend.Web.Views.ViewTypes
{
    public class MatchGameView
    {
        public Guid Id { get; }
        public IReadOnlyCollection<Guid> Players { get; private set; }
        public string Melding { get; private set; }
        public IEnumerable<Guid> MeldingPlayers { get; private set; }
        public int NumberOfTricks { get; private set; }
        public string NumberOfVips { get; private set; }

        public MatchGameView(Guid id, IReadOnlyCollection<Guid> players)
        {
            Id = id;
            Players = players;
            TeamTricks = new Dictionary<Guid, int>();
            Players = new List<Guid>();
            MeldingPlayers = new List<Guid>();
        }

        public void AddMelding(string melding, IEnumerable<Guid> meldingPlayers, int numberOfTricks, string numberOfVips)
        {
            NumberOfVips = numberOfVips;
            Melding = melding;
            MeldingPlayers = meldingPlayers;
            NumberOfTricks = numberOfTricks;
        }


        public void EndGame(Dictionary<Guid, int> teamTricks, decimal player1Score, decimal player2Score, decimal player3Score, decimal player4Score)
        {
            if (teamTricks == null)
            {
                
            }
            TeamTricks = teamTricks;
            Player1Score = player1Score;
            Player2Score = player2Score;
            Player3Score = player3Score;
            Player4Score = player4Score;
        }

        public Dictionary<Guid, int> TeamTricks { get; private set; }

        public decimal Player4Score { get; private set; }

        public decimal Player3Score { get; private set; }

        public decimal Player2Score { get; private set; }

        public decimal Player1Score { get; private set; }
    }
}
