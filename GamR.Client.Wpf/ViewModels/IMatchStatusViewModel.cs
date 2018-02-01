using System.Collections.ObjectModel;

namespace WpfApp1.ViewModels
{
    public interface IMatchStatusViewModel
    {
        ObservableCollection<PlayerStatusViewModel> PlayerStatusViewModels { get; }
    }
}