using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            Melding = new Melding(@event.Melding, @event.MeldingPlayerIds, @event.MeldingTeamPlayerIds, @event.NumberOfTricks, @event.NumberOfVips);
        }

        internal void Apply(GameEnded @event)
        {
            decimal player1Score = @event.Result.Values.ToList()[0];
            decimal player2Score = @event.Result.Values.ToList()[1];
            decimal player3Score = @event.Result.Values.ToList()[2];
            decimal player4Score = @event.Result.Values.ToList()[3];
            Result = new Result(player1Score, player2Score, player3Score, player4Score, @event.TeamTricks);
        }


        public Guid MatchId { get; private set; }

        public List<Guid> Players { get; private set; }

        
        public void AddMelding(string melding, IEnumerable<Guid> meldingPlayerIds, IEnumerable<Guid> meldingTeamPlayerIds, int numberOfTricks, string numberOfVips)
        {
            BaseApply(new Melded(Guid.NewGuid(), Id,MatchId, melding, meldingPlayerIds, meldingTeamPlayerIds, numberOfTricks, numberOfVips));
        }

        public void EndGame(decimal player1, decimal player2, decimal player3, decimal player4, int actualNumberOfTricks)
        {
            var results = new Dictionary<Guid, decimal>();
            results.Add(Players[0],player1);
            results.Add(Players[1],player2);
            results.Add(Players[2],player3);
            results.Add(Players[3],player4);
            var teamTricks = Melding.MeldingPlayerIds.ToDictionary(x => x, y => actualNumberOfTricks);
            BaseApply(new GameEnded(Guid.NewGuid(), Id, MatchId, results, teamTricks));
        }

        public void Reshuffle(string reason)
        {
            
        }

        public void EndGame(Dictionary<Guid, int> teamTricks)
        {
            
            var factor = 0.25m;
            var results = new Dictionary<Guid, decimal>();
            
            if (Melding.Type.EndsWith("SOL"))
            {
                foreach (var teamTrick in teamTricks)
                {
                    
                    var melderWin = true;
                    if (Melding.Type == "SOL")
                    {
                        factor = 6;
                        if (teamTrick.Value > 1)
                        {
                            factor *= -1;
                        }
                    }

                    if (Melding.Type == "REN SOL")
                    {
                        factor = 12;
                        if (teamTrick.Value > 0)
                        {
                            factor *= -1;
                        }
                    }
                    if (Melding.Type == "OPLÆGGER SOL")
                    {
                        factor = 24;
                        if (teamTrick.Value > 0)
                        {
                            factor *= -1;
                        }
                    }

                    if (results.ContainsKey(teamTrick.Key))
                    {
                        results[teamTrick.Key] += factor;
                    }
                    else
                    {
                        results.Add(teamTrick.Key, factor);
                    }
                    var otherPlayers = Players.Where(p => p != teamTrick.Key).ToList();
                    otherPlayers.ForEach(p =>
                    {
                        var amount = - factor / otherPlayers.Count;

                        if (results.ContainsKey(p))
                        {
                            results[p] += amount;
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
                if (new[] {"KLØR", "GRAN"}.Contains(Melding.Type))
                {
                    factor *= 2;
                }
                if (Melding.Type == "VIP")
                {
                    factor *= (decimal) Math.Pow(2, int.Parse(Melding.NumberOfVips));
                }
                var diff = teamTricks.Single().Value - Melding.NumberOfTricks;
                var money = factor * (diff == 0 ? 1 : diff);
                var otherPlayers = Players.Except(Melding.MeldingTeamPlayerIds).ToList();

                var factorForNumberOfMeldingPlayers = otherPlayers.Count / Melding.MeldingPlayerIds.Count();

                var meldingPlayerResults = Melding.MeldingTeamPlayerIds.Select(p => new {PlayerId = p, Result = money * factorForNumberOfMeldingPlayers});
                var otherPlayersResults = otherPlayers.Select(p => new {PlayerId = p, Result = -money});
                results = meldingPlayerResults
                    .Concat(otherPlayersResults)
                    .ToDictionary(x => x.PlayerId, y => y.Result);
            }

            BaseApply(new GameEnded(Guid.NewGuid(), Id, MatchId, results, teamTricks));

        }
    }
}