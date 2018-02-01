using System;

namespace GamR.Client.Wpf.Events
{
    public  class MatchCreated
    {
        public MatchCreated(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}