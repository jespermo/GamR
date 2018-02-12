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

        public ImmutableList<MatchGameView> GetById(Guid id)
        {
            if (!_games.ContainsKey(id)) return ImmutableList.Create<MatchGameView>();
            return _games[id].Values.ToImmutableList();
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
        
        public Task Handle(GameEnded args)
        {
            decimal player1Score = args.Result.Values.ToList()[0];
            decimal player2Score = args.Result.Values.ToList()[1];
            decimal player3Score = args.Result.Values.ToList()[2];
            decimal player4Score = args.Result.Values.ToList()[3];
            _games[args.MatchId][args.GameId].EndGame(args.TeamTricks, player1Score, player2Score, player3Score, player4Score);
            return Task.CompletedTask;
        }
    }
}