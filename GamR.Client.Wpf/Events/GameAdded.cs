using System;
using GamR.Client.Wpf.Models;

namespace GamR.Client.Wpf.Events
{
    public class GameAdded
    {
        public Guid MatchId { get; }

        public GameAdded(Guid matchId)
        {
            MatchId = matchId;
        }
    }
}