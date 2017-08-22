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
            foreach (var match in _matches)
            {
                match.AggregateScores = new List<AggregateResult>();
                foreach (var matchPlayer in match.Players)
                {
                    match.AggregateScores.Add(new AggregateResult(matchPlayer,match.Games.SelectMany(g=>g.Result.Where(res=>res.Player == matchPlayer)).Sum(res=>res.Result)));
                }
            }
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

        private List<PlayerResult> CreateResult(Random random)
        {
            var res = new List<PlayerResult>();
            var result = random.Next(2,100);
            var winner1 = _players[random.Next(3)];
            var nonwinners = _players.Where(p => p != winner1).ToArray();
            var winner2 = nonwinners[random.Next(2)];
            nonwinners = nonwinners.Where(p => p != winner2).ToArray();

            res.Add(new PlayerResult(winner1,result));
            res.Add(new PlayerResult(winner2,result));
            res.Add(new PlayerResult(nonwinners[0],-result));
            res.Add(new PlayerResult(nonwinners[1],-result));

            return res;
        }
    }

    internal class AggregateResult
    {
        public string MatchPlayer { get; }
        public decimal Sum { get; }

        public AggregateResult(string matchPlayer, decimal sum)
        {
            MatchPlayer = matchPlayer;
            Sum = sum;
        }
    }

    internal class PlayerResult
    {
        public string Player { get; }
        public decimal Result { get; }

        public PlayerResult(string player, decimal result)
        {
            Player = player;
            Result = result;
        }
    }

    internal class Match
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public List<Game> Games { get; set; }
        public string[] Players { get; set; }
        public List<AggregateResult> AggregateScores { get; set; }
    }

    internal class Game
    {
        public string MeldingPlayer { get; set; }
        public int MeldedTricks { get; set; }
        public string Melding { get; set; }
        public int NumberOfVips { get; set; }
        public List<PlayerResult> Result { get; set; }
        
    }
}