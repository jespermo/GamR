using System.Collections.ObjectModel;

namespace GamR.Client.Wpf.ViewModels.Interfaces
{
    public interface IGamesViewModel
    {
        ObservableCollection<string> Games { get; }
    }
}