using System;
using System.Collections.Generic;
using GamR.Backend.Web.Views.ViewTypes;

namespace GamR.Backend.Web.Views.ViewTypes
{
    public class MatchOverview
    {
        public Guid Id { get; }
        public DateTime Date { get; }
        public string Location { get; }

        public MatchOverview(Guid id, DateTime date, string location)
        {
            Id = id;
            Date = date;
            Location = location;
            Games = new Dictionary<Guid, MatchGameView>();
        }

        public void AddGame(MatchGameView gameView)
        {
            Games[gameView.Id] = gameView;
        }

        public Dictionary<Guid,MatchGameView> Games { get; }
    }
}