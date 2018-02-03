using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Web.Views
{
    public class ViewContainer : 
        ISubscribeToEvent<MatchCreated>, 
        ISubscribeToEvent<GameStarted>, 
        ISubscribeToEvent<Melded>, 
        ISubscribeToEvent<GameEnded>,
        ISubscribeToEvent<PlayerCreated>,
        ISubscribeToEvent<PlayerNameChanged>


    {
        public PlayersView PlayersView { get; }
        public MatchesView MatchesView { get; }
        public Dictionary<Guid, MatchView> MatchViews { get; }

        public ViewContainer()
        {
            PlayersView = new PlayersView();
            MatchesView = new MatchesView();
            MatchViews = new Dictionary<Guid, MatchView>();
        }

        public async Task Handle(MatchCreated args)
        {
            await MatchesView.Handle(args);
            
            var match = new MatchView();
            await match.Handle(args);
            MatchViews.Add(args.Id, match);
        }

        public async Task Handle(GameStarted args)
        {
            if (!MatchViews.TryGetValue(args.MatchId, out var match))
            {
                throw new Exception($"match not found by id: {args.MatchId}");
            }
            await match.Handle(args);
        }

        public async Task Handle(Melded args)
        {
            if (!MatchViews.TryGetValue(args.MatchId, out var match))
            {
                throw new Exception($"match not found by id: {args.MatchId}");
            }
            await match.Handle(args);
        }

        public async Task Handle(GameEnded args)
        {
            if (!MatchViews.TryGetValue(args.MatchId, out var match))
            {
                throw new Exception($"match not found by id: {args.MatchId}");
            }
            await match.Handle(args);
        }

        public async Task Handle(PlayerCreated args)
        {
            await PlayersView.Handle(args);
        }

        public async Task Handle(PlayerNameChanged args)
        {
            await PlayersView.Handle(args);
        }
    }
}