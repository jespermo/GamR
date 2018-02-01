using System.Threading.Tasks;

namespace GamR.Client.Wpf.Services
{
    public interface IRequester
    {
        Task<T> Get<T>(string path) where T : class;
        Task<T> Post<T>(object request, string path) where T : class;

    }
}
