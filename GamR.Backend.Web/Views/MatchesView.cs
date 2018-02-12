using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;
using GamR.Backend.Web.Views.ViewTypes;

namespace GamR.Backend.Web.Views
{
    public class MatchesView : ISubscribeToEvent<MatchCreated>
    {
        public MatchesView()
        {
            Matches = new Dictionary<Guid, MatchOverview>();
        }

        public Dictionary<Guid,MatchOverview> Matches { get;  }

        public Task Handle(MatchCreated args)
        {
            Matches.Add(args.Id,new MatchOverview(args.Id, args.Date, args.Location));
            return Task.CompletedTask;
        }

        public IImmutableList<MatchOverview> All() => Matches.Values.ToImmutableList();
        public MatchOverview ById(Guid matchId) => Matches[matchId];
    }
}