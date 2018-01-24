using System.Collections.ObjectModel;

namespace WpfApp1.ViewModels
{
    public interface IGamesViewModel
    {
        ObservableCollection<string> Games { get; }
    }
}