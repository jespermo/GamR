using System;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Events
{
    public class MatchCreated : Event
    {
        public Guid Id { get; }
        public DateTime Date { get; }
        public string Location { get; }

        public MatchCreated(Guid id, DateTime date, string location)
        {
            Id = id;
            Date = date;
            Location = location;
        }
    }
}