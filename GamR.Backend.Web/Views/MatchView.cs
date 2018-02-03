using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;
using GamR.Backend.Web.Views.ViewTypes;

namespace GamR.Backend.Web.Views
{
    public class MatchView : 
        ISubscribeToEvent<MatchCreated>, 
        ISubscribeToEvent<GameStarted>, 
        ISubscribeToEvent<Melded>, 
        ISubscribeToEvent<GameEnded>
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
}