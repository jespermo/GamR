using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}