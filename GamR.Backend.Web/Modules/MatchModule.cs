using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;

namespace GamR.Backend.Web.Modules
{
    public class MatchModule : NancyModule
    {
        private List<Match> _matches;

        public MatchModule()
        {
            CreateMatches();
            Get("/matches", args =>
                            {
                                var response = Response.AsJson<List<Match>>(_matches);
                                response.Headers.Add("Content-Type", "application/json");
                                return response;
                            });
            Get("/match/{id}", args =>
                               {
                                   return Enumerable.SingleOrDefault<Match>(_matches, p => p.Id == args.id);
                               });
        }

        private void CreateMatches()
        {
            _matches = new List<Match>
                       {
                           new Match
                           {
                               Id = 1,
                               Date = DateTime.Now,
                               Location = "Aarhus"
                           }
                       };
        }
    }

    public class Match
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
    }
}