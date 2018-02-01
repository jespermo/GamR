using System.Threading.Tasks;

namespace GamR.Client.Wpf.Services
{
    public interface IRequester
    {
        Task<T> Get<T>(string path);
        Task<T> Post<T>(object request, string path);

    }
}
