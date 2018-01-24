using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Aggregates;
using GamR.Backend.Core.Framework;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class MatchModule : NancyModule
    {
        private readonly Repository<Match> _matchRepository;

        public MatchModule(Repository<Match> matchRepository)
        {
            _matchRepository = matchRepository;
            //Get("/matches", args =>
            //                {
            //                    var response = Response.AsJson<List<Match>>(_matches);
            //                    response.Headers.Add("Content-Type", "application/json");
            //                    return response;
            //                });
            //Get("/match/{id}", args =>
            //                   {
            //                       return Enumerable.SingleOrDefault<Match>(_matches, p => p.Id == args.id);
            //                   });
            Post("/match", async args =>
            {
                var createMatch = this.Bind<CreateMatch>();
                var match = Match.Create(Guid.NewGuid(), createMatch.Date, createMatch.Location);
                await _matchRepository.Save(match);
                return Response.AsJson("");
            });
        }
    }

    public class CreateMatch
    {
        public DateTime Date { get; set; }

        public string Location { get; set; }
    }
}