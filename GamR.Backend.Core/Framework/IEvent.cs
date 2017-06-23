namespace GamR.Backend.Core.Framework
{
    public interface IEvent
    {
        int Version { get; }
    }

    public class Event : IEvent
    {
        public int Version { get; internal set; }
    }
}