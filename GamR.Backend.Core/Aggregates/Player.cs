using System;
using System.Reflection;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Aggregates
{
    public class Player : AggregateRoot
    {
        public override Guid Id => _id;
        private Guid _id;
        public string Name { get; private set; }
        
        public void Apply(PlayerCreated @event)
        {
            _id = @event.Id;
            Name = @event.Name;
        }

        public void Apply(PlayerNameChanged @event)
        {
            Name = @event.NewName;
        }

        public void ChangeName(string newName)
        {
            BaseApply(new PlayerNameChanged(_id, newName));
        }

        private Player() { }

        public static Player Create(Guid id, string name)
        {
            var player = new Player();
            player.BaseApply(new PlayerCreated(id, name));
            return player;
        }
    }
}
