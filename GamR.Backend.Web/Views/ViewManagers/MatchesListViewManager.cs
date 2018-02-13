using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;
using GamR.Backend.Web.Views.ViewTypes;

namespace GamR.Backend.Web.Views.ViewManagers
{

    public class MatchesListViewManager 
        : ListViewManager<MatchOverview>, 
        IViewManager,
        ISubscribeToEvent<MatchCreated>
    {
        public MatchesListViewManager()
        {
            _matches = new Dictionary<Guid, MatchOverview>();
        }

        private readonly Dictionary<Guid, MatchOverview> _matches;

        public Task Handle(Core.Events.MatchCreated args)
        {
            _matches.Add(args.Id, new MatchOverview(args.Id, args.Date, args.Location));
            return Task.CompletedTask;
        }

        public override IImmutableList<MatchOverview> All()
        {
            return _matches.Values.ToImmutableList();
        }

        public override IImmutableList<MatchOverview> Where(Func<MatchOverview, bool> predicate)
        {
            return _matches.Values.Where(predicate).ToImmutableList();
        }
    }
}