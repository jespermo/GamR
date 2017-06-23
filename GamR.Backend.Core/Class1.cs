using System;

namespace GamR.Backend.Core
{
    public class PlayerAggregate
    {
        public string Name { get; }

        private PlayerAggregate(string name)
        {
            Name = name;
        }

        static PlayerAggregate Create(string name)
        {
            return new PlayerAggregate(name);
        }
    }
}
