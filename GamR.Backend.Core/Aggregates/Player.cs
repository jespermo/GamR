using System;
using GamR.Backend.Core.Events;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Aggregates
{
    public class Player : AggregateRoot
    {
        public override Guid Id => _id;
        private Guid _id;
        public string Name { get; private set; }
        
        private void Apply(PlayerCreated @event)
        {
            _id = @event.Id;
            Name = @event.Name;
        }

        public Player(Guid id, string name)
        {
            Apply(new PlayerCreated(id, name));
        }
    }
}
