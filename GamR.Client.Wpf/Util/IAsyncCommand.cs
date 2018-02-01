using System.Threading.Tasks;
using System.Windows.Input;

namespace GamR.Client.Wpf.Util
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}