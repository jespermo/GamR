using System;
using System.Collections.Generic;

namespace GamR.Backend.Core.ValueTypes
{
    public class Result
    {
        public decimal Player1Score { get; }
        public decimal Player2Score { get; }
        public decimal Player3Score { get; }
        public decimal Player4Score { get; }
        public Dictionary<Guid, int> TeamTricks { get; }

        public Result(decimal player1Score, decimal player2Score, decimal player3Score, decimal player4Score, Dictionary<Guid, int> teamTricks)
        {
            Player1Score = player1Score;
            Player2Score = player2Score;
            Player3Score = player3Score;
            Player4Score = player4Score;
            TeamTricks = teamTricks;
        }
    }
}