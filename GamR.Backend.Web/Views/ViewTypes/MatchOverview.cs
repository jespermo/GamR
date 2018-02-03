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
            Games = new Dictionary<Guid, MatchGame>();
        }

        public void AddGame(MatchGame game)
        {
            Games[game.Id] = game;
        }

        public Dictionary<Guid,MatchGame> Games { get; }
    }
}