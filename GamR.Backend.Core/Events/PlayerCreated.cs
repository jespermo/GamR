using System;
using GamR.Backend.Core.Framework;

namespace GamR.Backend.Core.Events
{
    public class PlayerCreated : Event
    {
        public Guid Id { get; }
        public string Name { get; }

        public PlayerCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class PlayerNameChanged : Event
    {
        public Guid Id { get; }
        public string NewName { get; }

        public PlayerNameChanged(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }
}