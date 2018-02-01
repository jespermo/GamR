using System;
using System.Linq;
using GamR.Backend.Core.Framework;
using GamR.Backend.Web.ApiModels;
using Nancy;
using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class MatchModule : NancyModule
    {
        private readonly Repository<Core.Aggregates.Match> _matchRepository;
        private readonly MatchesView _matchesView;

        public MatchModule(Repository<Core.Aggregates.Match> matchRepository, MatchesView matchesView)
        {
            _matchRepository = matchRepository;
            _matchesView = matchesView;
            Get("/matches", args =>
                            {
                                var matches = _matchesView.Matches.Select(kvp =>
                                        new Match {Id = kvp.Key, Date = kvp.Value.Date, Location = kvp.Value.Location})
                                    .ToList();
                                var response = Response.AsJson(matches);
                                response.Headers.Add("Content-Type", "application/json");
                                return response;
                            });
            //Get("/match/{id}", args =>
            //                   {
            //                       return Enumerable.SingleOrDefault<Match>(_matches, p => p.Id == args.id);
            //                   });
            Post("/match", async args =>
            {
                var newMatch = this.Bind<NewMatch>();
                var match = Core.Aggregates.Match.Create(Guid.NewGuid(), newMatch.Date, newMatch.Location);
                await _matchRepository.Save(match);
                return Response.AsJson(match.Id);
            });
        }
    }


}