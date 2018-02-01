using System.Collections.ObjectModel;

namespace GamR.Client.Wpf.ViewModels.Interfaces
{
    public interface IMatchStatusViewModel
    {
        ObservableCollection<PlayerStatusViewModel> PlayerStatusViewModels { get; }
    }
}