using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;
using GamR.Backend.Core.Framework.Exceptions;

namespace GamR.Backend.Core.Aggregates
{
    public class Game :  AggregateRoot
    {
        public override Guid Id => _id;
        private Guid _id;

        public static Game StartNewGame(Guid id, Guid matchId, IEnumerable<Guid> playerIds)
        {
            var game = new Game();
            game.BaseApply(new GameStarted(id, matchId, playerIds));
            return game;
        }

        private Game()
        {
            Players = new List<Guid>();
        }

        public void Apply(GameStarted @event)
        {
            if (@event.Players.Count == 4)
            {
                _id = @event.Id;
                Players = @event.Players.ToList();
                MatchId = @event.MatchId;
            }
            else
            {
                throw new DomainException("Games has to be started with 4 plaeyrs exactly!");
            }
        }

        public Guid MatchId { get; private set; }

        public List<Guid> Players { get; private set; }

        public void AddMelding(string melding, Guid meldingPlayer, int numberOfTricks, string numberOfVips)
        {
            
        }

        public void EndGame()
        {
            
        }
    }
}