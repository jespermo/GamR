using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamR.Backend.Core.Aggregates;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Web
{
    class CsvEventLoader
    {
        private const int MeldingIndex = 8;
        private readonly Repository<Player> _playerRepository;
        private readonly Repository<Match> _matchRepository;
        private readonly Repository<Game> _gameRepository;
        public static Guid GameGuid;

        private const int PlayerOneScoreIndex = 2;
        private const int PlayerTwoScoreIndex = 3;
        private const int PlayerThreeScoreIndex = 4;
        private const int PlayerFourScoreIndex = 5;
        private const int PlayersRow = 2;
        private const int MeldingPlayerIndex = 10;
        private const int Players = 4;
        private const int NumberOfTricksIndex = 7;
        private const int NumberOfVipsIndex = 9;
        private const int ActualTricksIndex = 11;

        public CsvEventLoader(Repository<Game> gameRepository, Repository<Match> matchRepository, Repository<Player> playerRepository)
        {
            _gameRepository = gameRepository;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
        }

        internal async Task LoadEvents(string file)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var allLines = File.ReadAllLines($"{currentDir}\\..\\resources\\{file}", Encoding.UTF7);

            var separator = ';';
            var allLinesSplit = allLines.Select(l => l.Split(separator)).ToArray();
            
            var players = allLinesSplit[PlayersRow].Skip(2).Take(Players).Select(name => Player.Create(Guid.NewGuid(), name)).ToList();
            foreach (var player in players)
            {
                await _playerRepository.Save(player);
            }

            GameGuid = Guid.NewGuid();
            var match = Match.Create(GameGuid, DateTime.Parse(allLinesSplit[0][1]), allLinesSplit[0][4]);
            await _matchRepository.Save(match);

            var games = allLinesSplit.Skip(5)
                .TakeWhile(x => !x[0].Equals("STOP"))
                .Select(row => NewGame(row, match, players));

            foreach (var game in games)
            {
                await _gameRepository.Save(game);
            }
        }

        private static Game NewGame(string[] row, Match match, IEnumerable<Player> players)
        {
            try
            {

            var game = Game.StartNewGame(Guid.NewGuid(), match.Id, players.Select(x => x.Id));

            if (row[7] == "OMBLANDING")
            {
                game.Reshuffle("OMBLANDING");
                return game;
            }

            var meldingPlayerNames = row[MeldingPlayerIndex].Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
            var meldingPlayerIds = players
                .Where(player => meldingPlayerNames.Contains(player.Name))
                .Select(player => player.Id);

            var melding = row[MeldingIndex];
            game.AddMelding(melding, meldingPlayerIds, row[NumberOfTricksIndex].ValueOrZero(), row[NumberOfVipsIndex]);
            
            game.EndGame(decimal.Parse(row[PlayerOneScoreIndex]), decimal.Parse(row[PlayerTwoScoreIndex]), decimal.Parse(row[PlayerThreeScoreIndex]), decimal.Parse(row[PlayerFourScoreIndex]), row[ActualTricksIndex].ValueOrZero());
            return game;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        
    }
}