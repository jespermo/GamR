using System;
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
            int actualCountPlayersCreated = 0;
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
    }
}
