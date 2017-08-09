using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;
using GamR.Backend.Core.Framework.Exceptions;
using GamR.Backend.Core.ValueTypes;

namespace GamR.Backend.Core.Aggregates
{
    public class Game :  AggregateRoot
    {
        public override Guid Id => _id;
        public Melding Melding { get; private set; }
        public Result Result { get; private set; }

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

        internal void Apply(GameStarted @event)
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

        internal void Apply(Melded @event)
        {
            Melding = new Melding(@event.Melding, @event.MeldingPlayerIds, @event.NumberOfTricks, @event.NumberOfVips);
        }

        internal void Apply(GameEnded @event)
        {
            Result = new Result(@event.Player1Score, @event.Player2Score, @event.Player3Score, @event.Player4Score, @event.ActualNumberOfTricks);
        }


        public Guid MatchId { get; private set; }

        public List<Guid> Players { get; private set; }

        
        public void AddMelding(string melding, IEnumerable<Guid> meldingPlayerIds, int numberOfTricks, string numberOfVips)
        {
            BaseApply(new Melded(Guid.NewGuid(), Id, melding, meldingPlayerIds, numberOfTricks, numberOfVips));
        }

        public void EndGame(decimal player1, decimal player2, decimal player3, decimal player4, int actualNumberOfTricks)
        {
            BaseApply(new GameEnded(Guid.NewGuid(), Id, player1, player2, player3, player4, actualNumberOfTricks));
        }

        public void Reshuffle(string reason)
        {
            
        }
    }
}