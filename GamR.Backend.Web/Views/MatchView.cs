using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using GamR.Backend.Web.Views.ViewTypes;

namespace GamR.Backend.Web.Views
{
    public class MatchView
    {
        Dictionary<Guid, MatchGameView> _games = new Dictionary<Guid, MatchGameView>();
        public MatchView(DateTime date, string location, Guid id)
        {
            Date = date;
            Location = location;
            Id = id;
        }

        public DateTime Date { get; }
        public string Location { get; }
        public Guid Id { get; }
        public ImmutableList<MatchGameView> Games => _games.Values.ToImmutableList(); 
        
    }
}
