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
        private List<Guid> _players;

        public static Game StartNewGame(Guid id, IEnumerable<Guid> playerIds)
        {
            var game = new Game();
            game.Apply(new GameStarted(id, playerIds));
            return game;
        }

        private Game() { }

        private void Apply(GameStarted @event)
        {
            if (@event.Players.Count == 4)
            {
                _id = @event.Id;
                _players = @event.Players.ToList();
            }
            else
            {
                throw new DomainException("Games has to be started with 4 plaeyrs exactly!");
            }
        }

    }
}