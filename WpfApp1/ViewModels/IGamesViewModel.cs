using System.Collections.ObjectModel;

namespace WpfApp1.ViewModels
{
    public interface IGamesViewModel
    {
        ObservableCollection<IGame> Games { get; }
    }
}