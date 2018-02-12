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
    public class MatchGamesViewManger :
        IViewManager,
        ISubscribeToEvent<GameStarted>,
        ISubscribeToEvent<Melded>,
        ISubscribeToEvent<GameEnded>
    {
        private readonly Dictionary<Guid, Dictionary<Guid, MatchGameView>> _games;

        public MatchGamesViewManger()
        {
            _games = new Dictionary<Guid, Dictionary<Guid, MatchGameView>>();
        }

        private void EnsureMatch(Guid matchId)
        {
            _games.TryAdd(matchId, new Dictionary<Guid, MatchGameView>());
        }

        public async Task Handle(Core.Events.GameStarted args)
        {
            EnsureMatch(args.MatchId);
            _games[args.MatchId].Add(args.Id, new MatchGameView(args.Id, args.Players.ToList()));
        }

        public async Task Handle(Core.Events.Melded args)
        {
            _games[args.MatchId][args.GameId].AddMelding(args.Melding, args.MeldingPlayerIds, args.NumberOfTricks, args.NumberOfVips);
        }

        public async Task Handle(Core.Events.GameEnded args)
        {
            _games[args.MatchId][args.GameId].EndGame(args.ActualNumberOfTricks, args.Player1Score, args.Player2Score, args.Player3Score, args.Player4Score);
        }

        public IEnumerable<MatchGameView> GetById(Guid id)
        {
            return ImmutableList.ToImmutableList<MatchGameView>(_games[id].Values);
        }
    }
}