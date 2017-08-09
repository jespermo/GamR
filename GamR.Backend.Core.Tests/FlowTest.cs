using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Core.Aggregates;
using GamR.Backend.Core.Framework;
using GamR.Backend.Core.Framework.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using GamR.Backend.Core.Events;

namespace GamR.Backend.Core.Tests
{
    [TestClass]
    public class FlowTest
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // arrange
            var bus = new InMemoryBus();
            var eventStore = new InMemoryEventStore(bus);
            var simpleCountingPlayersView = new SimpleCountingPlayersView();
            await bus.Subscribe(simpleCountingPlayersView);

            var repo = new Repository<Player>(eventStore);

            // act
            await repo.Save(Player.Create(Guid.NewGuid(), "Ham der jelle"));
            await repo.Save(Player.Create(Guid.NewGuid(), "Ham der møjeren"));

            // assert
            Assert.AreEqual(2, simpleCountingPlayersView.PlayersCreated);

        }

        private class SimpleCountingPlayersView : ISubscribeToEvent<PlayerCreated>
        {
            public int PlayersCreated { get; private set; }
            public Task Handle(PlayerCreated args)
            {
                PlayersCreated++;
                return Task.CompletedTask;
            }
        }

        [TestMethod]
        public async Task MultipleEvents()
        {
            // arrange
            var bus = new InMemoryBus();
            var eventStore = new InMemoryEventStore(bus);
            var playersView = new PlayersView();
            await bus.Subscribe<PlayerCreated>(playersView);
            await bus.Subscribe<PlayerNameChanged>(playersView);

            var repo = new Repository<Player>(eventStore);

            // act
            var aggregate = Player.Create(Guid.NewGuid(), "Ham der jelle");
            aggregate.ChangeName("ham der jelle changed name");
            await repo.Save(aggregate);
            await repo.Save(Player.Create(Guid.NewGuid(), "Ham der møjeren"));

            // assert
            Assert.AreEqual(2, playersView.Players.Count());
            Assert.IsTrue(playersView.Players.SequenceEqual(new[] { "ham der jelle changed name", "Ham der møjeren" }));

        }


        private class PlayersView : ISubscribeToEvent<PlayerCreated>, ISubscribeToEvent<PlayerNameChanged>
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

            public IEnumerable<string> Players => _playerNames.Values;
        }
    }
}
