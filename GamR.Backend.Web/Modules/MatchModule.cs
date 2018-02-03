using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Framework;
using GamR.Backend.Web.ApiModels;
using GamR.Backend.Web.Views;
using Nancy;
using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class MatchModule : NancyModule
    {
        private readonly Repository<Core.Aggregates.Match> _matchRepository;
        private readonly ViewContainer _views;

        public MatchModule(Repository<Core.Aggregates.Match> matchRepository, ViewContainer views)
        {
            _matchRepository = matchRepository;
            _views = views;
            Get("/matches", args =>
            {
                var matches = _views.MatchesView.Matches.Select(kvp =>
                        new Match {Id = kvp.Key, Date = kvp.Value.Date, Location = kvp.Value.Location})
                    .ToList();
                var response = Response.AsJson(matches);
                response.Headers.Add("Content-Type", "application/json");
                return response;
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
                        NumberOfTrics = g.Value.NumberOfTricks,
                        NumberOfVips = g.Value.NumberOfVips,
                        ActualNumberOfTricks = g.Value.ActualNumberOfTricks
                    }
                ).ToList());
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
    }
    public class Game
    {
        public List<string> MeldingPlayers { get; set; }
        public string Melding { get; set; }
        public int NumberOfTrics { get; set; }
        public string NumberOfVips { get; set; }
        public int ActualNumberOfTricks { get; set; }
    }
}