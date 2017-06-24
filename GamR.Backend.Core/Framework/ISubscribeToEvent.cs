using System.Threading.Tasks;

namespace GamR.Backend.Core.Framework
{
    public interface ISubscribeToEvent<T> where T : IEvent
    {
        Task Handle(T args);
    }
}