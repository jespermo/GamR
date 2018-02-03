using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Web.Views
{
    public class PlayersView : ISubscribeToEvent<PlayerCreated>, ISubscribeToEvent<PlayerNameChanged>
    {
        private readonly Dictionary<Guid, string> _playerNames = new Dictionary<Guid, string>();

        public async Task Handle(PlayerCreated args)
        {
            _playerNames.Add(args.Id, args.Name);
        }

        public async Task Handle(PlayerNameChanged args)
        {
            _playerNames[args.Id] = args.NewName;
        }

        public Dictionary<Guid, string> Players => _playerNames;
    }
}