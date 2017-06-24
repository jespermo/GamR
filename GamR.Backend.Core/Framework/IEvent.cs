namespace GamR.Backend.Core.Framework
{
    public interface IEvent
    {
        int Version { get; set; }
    }

    public class Event : IEvent
    {
        public int Version { get; set; }
    }
}