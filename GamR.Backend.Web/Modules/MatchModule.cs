using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Framework;
using GamR.Backend.Web.ApiModels;
using GamR.Backend.Web.Views;
using GamR.Backend.Web.Views.ViewTypes;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class MatchModule : NancyModule
    {
        private readonly Repository<Core.Aggregates.Match> _matchRepository;
        private readonly ViewContainer _views;
        private readonly Repository<Core.Aggregates.Game> _gameRepository;

        public MatchModule(Repository<Core.Aggregates.Match> matchRepository, ViewContainer views, Repository<Core.Aggregates.Game> gameRepository)
        {
            _matchRepository = matchRepository;
            _views = views;
            _gameRepository = gameRepository;
            Get("/matches", args =>
            {
                var matches = _views.MatchesView.Matches.Select(kvp =>
                        new Match {Id = kvp.Key, Date = kvp.Value.Date, Location = kvp.Value.Location})
                    .ToList();
                var response = Response.AsJson(matches);
                response.Headers.Add("Content-Type", "application/json");
                return response;
            });

            Post("/match/{matchId}/game", async args =>
            {
                if (!Guid.TryParse(args.matchId, out Guid matchId))
                {
                    return HttpStatusCode.NotFound;
                }
                var newGame = this.Bind<Game>();
                var players = newGame.Players;
                Core.Aggregates.Game game = Core.Aggregates.Game.StartNewGame(Guid.NewGuid(),matchId, players.Select(p => p.Id));
                var meldingPlayerIds = players.Where(p => newGame.MeldingPlayers.Any(mp => mp == p.Name)).Select(x => x.Id);
                var meldingTeamPlayerIds = newGame.MeldingTeam.Select(x=>x.Id);
                game.AddMelding(newGame.Melding, meldingPlayerIds, meldingTeamPlayerIds,
                    newGame.NumberOfTricks,newGame.NumberOfVips);
                
                game.EndGame(newGame.TeamTricks.ToDictionary(x => x.TeamId, y => y.Result));
                await _gameRepository.Save(game);
                return Response.AsJson(game.Id);
            });

            Get("/match/{matchId}/games", args =>
            {
                if (!Guid.TryParse(args.matchId, out Guid matchId))
                {
                    return HttpStatusCode.NotFound;
                }
                 
                var match = _views.MatchViews[matchId];
                var players = _views.PlayersView.Players;
                var response = Response.AsJson(match.Games.Select(g =>
                    new Game
                    {
                        MeldingPlayers = g.Value.MeldingPlayers?.Select(x=>players[x]).ToList(),
                        Melding = g.Value.Melding,
                        NumberOfTricks = g.Value.NumberOfTricks,
                        NumberOfVips = g.Value.NumberOfVips,
                        TeamTricks = g.Value.TeamTricks.Select(tt =>new TeamTricks{TeamId = tt.Key, Result = tt.Value}).ToList(),
                        Players = CreateScore(g.Value,players)                        }
                ).ToList());
                response.Headers.Add("Content-Type", "application/json");
                return response;
            });

            Get("/match/{matchId}/status", args =>
            {
                if (!Guid.TryParse(args.matchId, out Guid matchId))
                {
                    return HttpStatusCode.NotFound;
                }

                var match = _views.MatchViews[matchId];
                var players = _views.PlayersView.Players;
                var playerGuids = match.Games.Values.FirstOrDefault(x => x.Players?.Count == 4)?.Players?.ToList() ?? new List<Guid>
                {
                    _views.PlayersView.Players.Keys.ToList()[0],
                    _views.PlayersView.Players.Keys.ToList()[1],
                    _views.PlayersView.Players.Keys.ToList()[2],
                    _views.PlayersView.Players.Keys.ToList()[3],

                };
                var p1Score = new PlayerScore
                {
                    Score = match.Games.Sum(g => g.Value.Player1Score),
                    Name = players[playerGuids[0]]
                };
                var p2Score = new PlayerScore
                {
                    Score = match.Games.Sum(g => g.Value.Player2Score),
                    Name = players[playerGuids[1]]
                };

                var p3Score = new PlayerScore
                {
                    Score = match.Games.Sum(g => g.Value.Player3Score),
                    Name = players[playerGuids[2]]
                };

                var p4Score = new PlayerScore
                {
                    Score = match.Games.Sum(g => g.Value.Player4Score),
                    Name = players[playerGuids[3]]
                };
                var response = Response.AsJson(
                    new MatchStatus
                    {
                        PlayerStatus = new List<PlayerScore>
                        {
                            p1Score,
                            p2Score,
                            p3Score,
                            p4Score
                        }
                    }
                );
                response.Headers.Add("Content-Type", "application/json");
                return response;
            });

            Post("/match", async args =>
            {
                var newMatch = this.Bind<NewMatch>();
                var match = Core.Aggregates.Match.Create(Guid.NewGuid(), newMatch.Date, newMatch.Location);
                await _matchRepository.Save(match);
                return Response.AsJson(match.Id);
            });
        }

        private List<PlayerScore> CreateScore(MatchGame match, Dictionary<Guid, string> players)
        {
            var playerScores = new List<PlayerScore>();
            var playerGuids = players.Keys.ToList();
            playerScores.Add(new PlayerScore
            {
                Id = playerGuids[0], Name = players[playerGuids[0]], Score = match.Player1Score
            });
            playerScores.Add(new PlayerScore
            {
                Id = playerGuids[1], Name = players[playerGuids[1]], Score = match.Player2Score
            });
            playerScores.Add(new PlayerScore
            {
                Id = playerGuids[2], Name = players[playerGuids[2]], Score = match.Player3Score
            });
            playerScores.Add(
                new PlayerScore
                {
                    Id = playerGuids[3], Name = players[playerGuids[3]], Score = match.Player4Score
                });
            return playerScores;
        }
    
    }

    public class MatchStatus
    {
        public List<PlayerScore> PlayerStatus { get; set; }
    }

    public class Game
    {
        public List<string> MeldingPlayers { get; set; }
        public string Melding { get; set; }
        public int NumberOfTricks { get; set; }
        public string NumberOfVips { get; set; }
        public List<TeamTricks> TeamTricks { get; set; }

        public List<PlayerScore> MeldingTeam { get; set; }

        public List<PlayerScore> Players {get; set; }
    }

    public class TeamTricks
    {
        public Guid TeamId { get; set; }
        public int Result { get; set; }
    }

    public class PlayerScore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Score { get; set; }
    }
}