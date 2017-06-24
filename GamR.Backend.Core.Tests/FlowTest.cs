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
        public void TestMethod1()
        {
            // arrange
            var bus = new InMemoryBus();
            var eventStore = new InMemoryEventStore(bus);
            var simpleCountingPlayersView = new SimpleCountingPlayersView();
            bus.Subscribe(simpleCountingPlayersView).Wait();

            var repo = new Repository<Player>(eventStore);

            // act
            repo.Save(Player.Create(Guid.NewGuid(), "Ham der jelle")).Wait();
            repo.Save(Player.Create(Guid.NewGuid(), "Ham der møjeren")).Wait();

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
        public void MultipleEvents()
        {
            // arrange
            var bus = new InMemoryBus();
            var eventStore = new InMemoryEventStore(bus);
            var playersView = new PlayersView();
            bus.Subscribe<PlayerCreated>(playersView).Wait();
            bus.Subscribe<PlayerNameChanged>(playersView).Wait();

            var repo = new Repository<Player>(eventStore);

            // act
            var aggregate = Player.Create(Guid.NewGuid(), "Ham der jelle");
            aggregate.Apply(new PlayerNameChanged(aggregate.Id, "ham der jelle changed name"));
            repo.Save(aggregate).Wait();
            repo.Save(Player.Create(Guid.NewGuid(), "Ham der møjeren")).Wait();

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
