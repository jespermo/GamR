using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Web.Views.ViewManagers
{
    public class MatchPlayerStatusViewManager :
        IViewManager,
        ISubscribeToEvent<GameEnded>,
        ISubscribeToEvent<MatchCreated>,
        ISubscribeToEvent<PlayerCreated>
    {
        private readonly Dictionary<Guid,Dictionary<Guid,PlayerStatus>> _matches = new Dictionary<Guid, Dictionary<Guid, PlayerStatus>>();
        private readonly Dictionary<Guid, string> _players = new Dictionary<Guid, string>();

        public ImmutableList<PlayerStatus> GetPlayerStatusses(Guid matchId)
        {
            return _matches[matchId].Select(p => p.Value).ToImmutableList();
        }

        public Task Handle(GameEnded args)
        {
            foreach (var result in args.Result)
            {
                if (!_matches[args.MatchId].TryGetValue(result.Key, out var playerStatus))
                {
                    playerStatus = new PlayerStatus{Name = _players[result.Key], Score = 0};
                    _matches[args.MatchId].Add(result.Key,playerStatus);
                }
                playerStatus.Score += result.Value;
            }
            return Task.CompletedTask;
        }

        public Task Handle(MatchCreated args)
        {
            _matches.Add(args.Id, new Dictionary<Guid, PlayerStatus>());
            return Task.CompletedTask;
        }

        public Task Handle(PlayerCreated args)
        {
            _players.Add(args.Id, args.Name);
            return Task.CompletedTask;
        }
    }

    public class PlayerStatus
    {
        public string Name { get; set; }

        public decimal Score { get; set; }
    }
}