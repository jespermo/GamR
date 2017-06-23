using System;

namespace GamR.Backend.Core.Events
{
    public class PlayerCreated
    {
        public Guid Id { get; }
        public string Name { get; }

        public PlayerCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}