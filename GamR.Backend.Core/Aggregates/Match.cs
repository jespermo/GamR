using System;
using System.Reflection;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Aggregates
{
    public class Match : AggregateRoot
    {
        private Guid _id;
        public override Guid Id => _id;

        public void Apply(MatchCreated @event)
        {
            _id = @event.Id;
            Date = @event.Date;
            Location = @event.Location;
        }

        public DateTime Date { get; private set; }

        public string Location { get; private set; }

        public static Match Create(Guid id, DateTime date, string location)
        {
            var match = new Match();
            match.BaseApply(new MatchCreated(id, date, location));
            return match;
        }
    }
}