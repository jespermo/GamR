using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;
using Microsoft.CodeAnalysis;

namespace GamR.Backend.Web
{
    public class MatchesView : ISubscribeToEvent<MatchCreated>
    {
        public MatchesView()
        {
            Matches = new Dictionary<Guid, MatchOverview>();
        }

        public Dictionary<Guid,MatchOverview> Matches { get;  }

        public Task Handle(MatchCreated args)
        {
            Matches.Add(args.Id,new MatchOverview(args.Id, args.Date, args.Location));
            return Task.CompletedTask;
        }
    }

    public class MatchView : ISubscribeToEvent<MatchCreated>, ISubscribeToEvent<GameStarted>, ISubscribeToEvent<Melded>, ISubscribeToEvent<GameEnded>
    {
        public MatchView()
        {
            Games = new Dictionary<Guid, MatchGame>();
        }

        public Task Handle(MatchCreated args)
        {
            Id = args.Id;
            Date = args.Date;
            Location = args.Location;
            return Task.CompletedTask;
        }

        public DateTime Date { get; private set; }
        public string Location { get; private set; }
        public Guid Id { get; private set; }
        public Dictionary<Guid, MatchGame> Games { get; }

        public Task Handle(GameStarted args)
        {
            Games.Add(args.Id, new MatchGame(args.Id, args.Players));
            return Task.CompletedTask;;
        }

        public Task Handle(Melded args)
        {
            Games[args.GameId].AddMelding(args.Melding, args.MeldingPlayerIds, args.NumberOfTricks, args.NumberOfVips);
            return Task.CompletedTask;
        }

        public Task Handle(GameEnded args)
        {
            Games[args.GameId].EndGame(args.ActualNumberOfTricks,args.Player1Score, args.Player2Score, args.Player3Score, args.Player4Score);
            return Task.CompletedTask;
        }
    }

    public class GameMelding
    {
        private readonly string _numberOfVips;
        public string Melding { get; }
        public IEnumerable<Guid> MeldingPlayers { get; }
        public int NumberOfTricks { get; }

        public GameMelding(string melding, IEnumerable<Guid> meldingPlayers, int numberOfTricks, string numberOfVips)
        {
            _numberOfVips = numberOfVips;
            Melding = melding;
            MeldingPlayers = meldingPlayers;
            NumberOfTricks = numberOfTricks;
        }
    }

    public class MatchGame
    {
        public Guid Id { get; }
        public IReadOnlyCollection<Guid> Players { get; private set; }
        public string Melding { get; private set; }
        public IEnumerable<Guid> MeldingPlayers { get; private set; }
        public int NumberOfTricks { get; private set; }
        public string NumberOfVips { get; private set; }

        public MatchGame(Guid id, IReadOnlyCollection<Guid> players)
        {
            Id = id;
            Players = players;
        }

        public void AddMelding(string melding, IEnumerable<Guid> meldingPlayers, int numberOfTricks, string numberOfVips)
        {
            NumberOfVips = numberOfVips;
            Melding = melding;
            MeldingPlayers = meldingPlayers;
            NumberOfTricks = numberOfTricks;
        }


        public void EndGame(int actualNumberOfTricks, decimal player1Score, decimal player2Score, decimal player3Score, decimal player4Score)
        {
            ActualNumberOfTricks = actualNumberOfTricks;
            Player1Score = player1Score;
            Player2Score = player2Score;
            Player3Score = player3Score;
            Player4Score = player4Score;
        }

        public int ActualNumberOfTricks { get; private set; }

        public decimal Player4Score { get; private set; }

        public decimal Player3Score { get; private set; }

        public decimal Player2Score { get; private set; }

        public decimal Player1Score { get; private set; }
    }

    public class MatchOverview
    {
        public Guid Id { get; }
        public DateTime Date { get; }
        public string Location { get; }

        public MatchOverview(Guid id, DateTime date, string location)
        {
            Id = id;
            Date = date;
            Location = location;
            Games = new Dictionary<Guid, MatchGame>();
        }

        public void AddGame(MatchGame game)
        {
            Games[game.Id] = game;
        }

        public Dictionary<Guid,MatchGame> Games { get; }
    }

}