using System;
using System.Collections.Generic;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Events
{
    public class GameEnded : Event
    {
        public Guid Id { get; }
        public Guid GameId { get; }
        public Guid MatchId { get; }
        public Dictionary<Guid, decimal> Result { get; }

        //public decimal Player1Score { get; }
        //public decimal Player2Score { get; }
        //public decimal Player3Score { get; }
        //public decimal Player4Score { get; }
        public int ActualNumberOfTricks { get; }


        public GameEnded(Guid id, Guid gameId, Guid matchId, Dictionary<Guid,decimal> result, int actualNumberOfTricks)
        {
            Id = id;
            GameId = gameId;
            MatchId = matchId;
            Result = result;
            //Player1Score = player1Score;
            //Player2Score = player2Score;
            //Player3Score = player3Score;
            //Player4Score = player4Score;
            ActualNumberOfTricks = actualNumberOfTricks;
        }
    }
}