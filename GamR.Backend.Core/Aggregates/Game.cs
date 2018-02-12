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
            decimal player1Score = @event.Result.Values.ToList()[0];
            decimal player2Score = @event.Result.Values.ToList()[1];
            decimal player3Score = @event.Result.Values.ToList()[2];
            decimal player4Score = @event.Result.Values.ToList()[3];
            Result = new Result(player1Score, player2Score, player3Score, player4Score, @event.ActualNumberOfTricks);
        }


        public Guid MatchId { get; private set; }

        public List<Guid> Players { get; private set; }

        
        public void AddMelding(string melding, IEnumerable<Guid> meldingPlayerIds, int numberOfTricks, string numberOfVips)
        {
            BaseApply(new Melded(Guid.NewGuid(), Id,MatchId, melding, meldingPlayerIds, numberOfTricks, numberOfVips));
        }

        public void EndGame(decimal player1, decimal player2, decimal player3, decimal player4, int actualNumberOfTricks)
        {
            var results = new Dictionary<Guid, decimal>();
            results.Add(Players[0],player1);
            results.Add(Players[1],player2);
            results.Add(Players[2],player3);
            results.Add(Players[3],player4);
            BaseApply(new GameEnded(Guid.NewGuid(), Id, MatchId, results, actualNumberOfTricks));
        }

        public void Reshuffle(string reason)
        {
            
        }

        public void EndGame(int actualNumberOfTricks)
        {
            var factor = 0.25m;
            var results = new Dictionary<Guid, decimal>();
            if (Melding.Type.EndsWith("SOL"))
            {
                var melderWin = true;
                if (Melding.Type == "SOL")
                {
                    factor = 6;
                    if (actualNumberOfTricks > 1)
                    {
                        factor *= -1;
                    }
                }

                if (Melding.Type == "REN SOL")
                {
                    factor = 12;
                    if (actualNumberOfTricks > 0)
                    {
                        factor *= -1;
                    }
                }

                if (Melding.Type == "OPLÆGGER SOL")
                {
                    factor = 24;
                    if (actualNumberOfTricks > 0)
                    {
                        factor *= -1;
                    }
                }
                foreach (var meldingPlayerId in Melding.MeldingPlayerIds)
                {
                    results.Add(meldingPlayerId,factor);
                    var otherPlayers = Players.Where(p => p != meldingPlayerId).ToList();
                    otherPlayers.ForEach(p =>
                    {
                        var amount = factor / otherPlayers.Count;

                        if (results.TryGetValue(p, out var playerEntry))
                        {
                            playerEntry += amount;
                        }
                        else
                        {
                            results.Add(p, amount);
                        }
                    });
                }
            }
            else
            {
                for (int i = 7; i < 13; i++)
                {
                    if (i == Melding.NumberOfTricks)
                    {
                        break;
                    }
                    factor *= 2;
                }
                var money = factor * (actualNumberOfTricks - Melding.NumberOfTricks);
                results = Melding.MeldingPlayerIds.Select(p => new {PlayerId = p, Result = money})
                    .Concat(Players.Except(Melding.MeldingPlayerIds).Select(p => new {PlayerId = p, Result = -money}))
                    .ToDictionary(x => x.PlayerId, y => y.Result);
            }

            BaseApply(new GameEnded(Guid.NewGuid(), Id, MatchId, results, actualNumberOfTricks));

        }
    }
}