namespace GamR.Backend.Core.ValueTypes
{
    public class Result
    {
        public decimal Player1Score { get; }
        public decimal Player2Score { get; }
        public decimal Player3Score { get; }
        public decimal Player4Score { get; }
        public int ActualNumberOfTricks { get; }

        public Result(decimal player1Score, decimal player2Score, decimal player3Score, decimal player4Score, int actualNumberOfTricks)
        {
            Player1Score = player1Score;
            Player2Score = player2Score;
            Player3Score = player3Score;
            Player4Score = player4Score;
            ActualNumberOfTricks = actualNumberOfTricks;
        }
    }
}