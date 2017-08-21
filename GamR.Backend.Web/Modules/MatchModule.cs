using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nancy;

namespace GamR.Backend.Web.Modules
{
    public class MatchModule : NancyModule
    {
        private List<Match> _matches;
        private string[] _players;

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
            _players = new []{"Asbjørn", "Søren", "Baastrup","Møjbæk"};
            _matches = new List<Match>
                       {
                           new Match
                           {
                               Id = 1,
                               Date = DateTime.Now,
                               Location = "Aarhus",
                               Games = CreateGames(),
                               Players = _players,

                           }
                       };
        }

        private List<Game> CreateGames()
        {
            var meldings = new[] {"KLØR", "TRUMFFRI", "TRUMF", "VIP"};
            return Enumerable.Range(0, 100)
                .Select(i =>
                        {
                            var random = new Random(i);
                            var melding = meldings[random.Next(meldings.Length - 1)];
                            return new Game
                                   {
                                       MeldingPlayer = _players[random.Next(3)],
                                       Melding = melding,
                                       NumberOfVips = melding == "VIP" ? random.Next(3) + 1 : 0,
                                       MeldedTricks = new[] {8, 9, 10, 11, 12, 13}[random.Next(5)],
                                       Result = CreateResult(random)

                            };
                        }).ToList();
        }

        private Dictionary<string, decimal> CreateResult(Random random)
        {
            var dic = new Dictionary<string, decimal>();
            var result = random.Next(2,100);
            var winner1 = _players[random.Next(3)];
            var nonwinners = _players.Where(p => p != winner1).ToArray();
            var winner2 = nonwinners[random.Next(2)];
            nonwinners = nonwinners.Where(p => p != winner2).ToArray();

            dic.Add(winner1,result);
            dic.Add(winner2,result);

            dic.Add(nonwinners[0], -result);
            dic.Add(nonwinners[1], -result);

            return dic;
        }
    }

    internal class Match
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public List<Game> Games { get; set; }
        public string[] Players { get; set; }
    }

    internal class Game
    {
        public string MeldingPlayer { get; set; }
        public int MeldedTricks { get; set; }
        public string Melding { get; set; }
        public int NumberOfVips { get; set; }
        public Dictionary<string,decimal> Result { get; set; }
        
    }
}