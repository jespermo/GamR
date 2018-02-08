using System.Collections.ObjectModel;
using GamR.Client.Wpf.Models;

namespace GamR.Client.Wpf.ViewModels.Interfaces
{
    public interface IGamesViewModel
    {
        ObservableCollection<Game> Games { get; }
    }
}