using System;
using System.Collections.Generic;

namespace GamR.Backend.Web.Views.ViewTypes
{
    public class MatchGame
    {
        public Guid Id { get; }
        public IReadOnlyCollection<Guid> Players { get; private set; }
        public string Melding { get; private set; }
        public IEnumerable<Guid> MeldingPlayers { get; private set; }
        public int NumberOfTricks { get; private set; }
        public string NumberOfVips { get; private set; }

        public MatchGame(Guid id, IReadOnlyCollection<Guid> players)
        {
            Id = id;
            Players = players;
        }

        public void AddMelding(string melding, IEnumerable<Guid> meldingPlayers, int numberOfTricks, string numberOfVips)
        {
            NumberOfVips = numberOfVips;
            Melding = melding;
            MeldingPlayers = meldingPlayers;
            NumberOfTricks = numberOfTricks;
        }


        public void EndGame(int actualNumberOfTricks, decimal player1Score, decimal player2Score, decimal player3Score, decimal player4Score)
        {
            ActualNumberOfTricks = actualNumberOfTricks;
            Player1Score = player1Score;
            Player2Score = player2Score;
            Player3Score = player3Score;
            Player4Score = player4Score;
        }

        public int ActualNumberOfTricks { get; private set; }

        public decimal Player4Score { get; private set; }

        public decimal Player3Score { get; private set; }

        public decimal Player2Score { get; private set; }

        public decimal Player1Score { get; private set; }
    }
}