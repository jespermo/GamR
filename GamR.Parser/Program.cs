using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GamR.Backend.Core.Aggregates;
using GamR.Backend.Core.Framework;
using GamR.Backend.Core.Framework.Impl;

namespace GamR.Parser
{
    class Program
    {
        private static char _separator;

        static Dictionary<int, string> rows = new Dictionary<int, string>();
        private static Repository<Player> _playerRepository;
        private static Repository<Backend.Core.Aggregates.Match> _matchRepository;
        private static Repository<Backend.Core.Aggregates.Game> _gameRepository;

        static void Main(string[] args)
        {
            var match = ParseFile();
            CreateRepositories();
            CreateData(match).RunSynchronously();
        }

        private static void CreateRepositories()
        {
            var inMemoryEventStore = new InMemoryEventStore(new InMemoryBus());
            _playerRepository = new Repository<Player>(inMemoryEventStore);
            _matchRepository = new Repository<Backend.Core.Aggregates.Match>(inMemoryEventStore);
            _gameRepository = new Repository<Backend.Core.Aggregates.Game>(inMemoryEventStore);
        }

        private static async Task CreateData(Match match)
        {
            var players = match.Players;
            var playerAggregateIds = new Dictionary<string,Guid>();
            foreach (var player in players)
            {
               var aggregatePlayer = Player.Create(Guid.NewGuid(), player);
               await  _playerRepository.Save(aggregatePlayer);
               playerAggregateIds.Add(aggregatePlayer.Name,aggregatePlayer.Id);
            }
            
            var aggregateMatch = Backend.Core.Aggregates.Match.Create(Guid.NewGuid(), match.Date, match.Location);
            await _matchRepository.Save(aggregateMatch);
            foreach (var game in match.Games)
            {
                var aggregateGame = Backend.Core.Aggregates.Game.StartNewGame(Guid.NewGuid(), aggregateMatch.Id, playerAggregateIds.Values);
                var gameMelding = game.Melding;
                var filteredPlayerIds = playerAggregateIds.Where(x=>x.Key == gameMelding.Melder)
                    .Select(x=>x.Value);
                var teamIds = 
                aggregateGame.AddMelding(gameMelding.GameMelding, filteredPlayerIds,teamIds, gameMelding.NumberOfTricks, gameMelding.NumberOfVips);
                //aggregateGame.EndGame();
                await _gameRepository.Save(aggregateGame);
            }
        }


        private static Match ParseFile()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var allLines = File.ReadAllLines($"{currentDir}\\..\\resources\\whist.csv");

            _separator = ';';
            var allLinesSplit = allLines.Select(l => l.Split(_separator));
            int rowNumber = 0;
            var match = new Match();
            foreach (var line in allLinesSplit)
            {
                rowNumber++;
                if (line.All(x => string.IsNullOrWhiteSpace(x) || x == "0")) continue;
                if (rowNumber == 1) //Date and Location
                {
                    match.Date = DateTime.Parse(line[1]);
                    match.Location = line[4];
                }
                if (rowNumber == 3) //Players
                {
                    match.Players.Add(line[2]);
                    match.Players.Add(line[3]);
                    match.Players.Add(line[4]);
                    match.Players.Add(line[5]);
                }
                if (rowNumber == 4) //Total results
                {
                    //Wait
                }
                if (rowNumber == 5) //Melding headers
                {
                }
                if (rowNumber >= 7) //Games and meldings
                {
                    AddGame(match, line);
                }
            }
            return match;
        }

        private static void AddGame(Match match, string[] row)
        {
            var game = new Game();
            if (row[7] == "OMBLANDING")
            {
                game.Invalid = "OMBLANDING";
                match.Games.Add(game);
                return;
            }
            game.Results = new Dictionary<string, decimal>();
            
            
            game.Results.Add(match.Players[0],decimal.Parse(row[2]));
            game.Results.Add(match.Players[1],decimal.Parse(row[3]));
            game.Results.Add(match.Players[2],decimal.Parse(row[4]));
            game.Results.Add(match.Players[3],decimal.Parse(row[5]));

            game.Melding = new Melding(row[7], row[8], row[9], row[10], row[11]);
            match.Games.Add(game);
        }
    }

    internal class Melding
    {
        public int NumberOfTricks { get; }
        public string GameMelding { get; }
        public string NumberOfVips { get; }
        public string Melder { get; }
        public int NumberOfTricksAchieved { get; }

        public Melding(string numberOfTricks, string melding, string numberOfVips, string melder, string numberOfTricksAchieved)
        {
            NumberOfTricks = !string.IsNullOrEmpty(numberOfTricks) ? int.Parse(numberOfTricks) : 0;
            GameMelding = melding;
            NumberOfVips = !string.IsNullOrEmpty(numberOfVips) ? numberOfVips : "-";
            Melder = melder;
            NumberOfTricksAchieved = !string.IsNullOrEmpty(numberOfTricksAchieved) ? int.Parse(numberOfTricksAchieved) : 0;
        }
    }

    internal class Game
    {
        public Dictionary<string,decimal> Results { get; set; }
        public Melding Melding { get; set; }
        public string Invalid { get; set; }
    }

    internal class Match
    {
        public Match()
        {
            Players = new List<string>();
            Games = new List<Game>();
        }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public List<string> Players { get; set; }
        public List<Game> Games { get; set; }
    }
}