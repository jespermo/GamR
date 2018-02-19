using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Framework;
using GamR.Backend.Web.ApiModels;
using GamR.Backend.Web.Views;
using GamR.Backend.Web.Views.ViewManagers;
using GamR.Backend.Web.Views.ViewTypes;
using Nancy;
using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class MatchModule : NancyModule
    {
        public MatchModule(
            Repository<Core.Aggregates.Match> matchRepository,
            MatchGamesViewManger matchGamesViewManager,
            MatchesListViewManager matchesListViewManager,
            MatchPlayerStatusViewManager matchplayerStatusViewManager,
            PlayersViewManager playersViewManager,
            Repository<Core.Aggregates.Game> gameRepository)
        {
            Get("/matches", args =>
            {
                var matches = matchesListViewManager.All().Select(match =>
                        new Match { Id = match.Id, Date = match.Date, Location = match.Location })
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
                Core.Aggregates.Game game = Core.Aggregates.Game.StartNewGame(Guid.NewGuid(), matchId, players.Select(p => p.Id));
                var meldingPlayerIds = players.Where(p => newGame.MeldingPlayers.Any(mp => mp == p.Name)).Select(x => x.Id);
                var meldingTeamPlayerIds = newGame.MeldingTeam.Select(x => x.Id);
                game.AddMelding(newGame.Melding, meldingPlayerIds, meldingTeamPlayerIds,
                    newGame.NumberOfTricks, newGame.NumberOfVips);

                game.EndGame(newGame.TeamTricks.ToDictionary(x => x.TeamId, y => y.Result));
                await gameRepository.Save(game);
                return Response.AsJson(game.Id);
            });

            Get("/match/{matchId}/games", args =>
            {
                if (!Guid.TryParse(args.matchId, out Guid matchId))
                {
                    return HttpStatusCode.NotFound;
                }

                var games = matchGamesViewManager.GetById(matchId);
                var players = playersViewManager.Players;
                var response = Response.AsJson(games.Select(g =>
                    new Game
                    {
                        MeldingPlayers = g.MeldingPlayers?.Select(x => players[x]).ToList(),
                        Melding = g.Melding,
                        NumberOfTricks = g.NumberOfTricks,
                        NumberOfVips = g.NumberOfVips,
                        TeamTricks = g.TeamTricks.Select(tt => new TeamTricks { TeamId = tt.Key, Result = tt.Value }).ToList(),
                        Players = CreateScore(g, players)
                    }
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
                var response = Response.AsJson(
                    new MatchStatus
                    {
                        PlayerStatus = new List<PlayerScore>(matchplayerStatusViewManager.GetPlayerStatusses(matchId)
                        .Select(ps => new PlayerScore{Name = ps.Name, Score = ps.Score}))
                        .ToList()
                    }
                );
                response.Headers.Add("Content-Type", "application/json");
                return response;
            });

            Post("/match", async args =>
            {
                var newMatch = this.Bind<NewMatch>();
                var match = Core.Aggregates.Match.Create(Guid.NewGuid(), newMatch.Date, newMatch.Location);
                await matchRepository.Save(match);
                return Response.AsJson(match.Id);
            });
        }

        private List<PlayerScore> CreateScore(MatchGameView match, Dictionary<Guid, string> players)
        {
            var playerScores = new List<PlayerScore>();
            var playerGuids = players.Keys.ToList();
            playerScores.Add(new PlayerScore
            {
                Id = playerGuids[0],
                Name = players[playerGuids[0]],
                Score = match.Player1Score
            });
            playerScores.Add(new PlayerScore
            {
                Id = playerGuids[1],
                Name = players[playerGuids[1]],
                Score = match.Player2Score
            });
            playerScores.Add(new PlayerScore
            {
                Id = playerGuids[2],
                Name = players[playerGuids[2]],
                Score = match.Player3Score
            });
            playerScores.Add(
                new PlayerScore
                {
                    Id = playerGuids[3],
                    Name = players[playerGuids[3]],
                    Score = match.Player4Score
                });
            return playerScores;
        }

    }

   
}
