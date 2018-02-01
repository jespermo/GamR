using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class GamesViewModel : ViewModelBase, IGamesViewModel
    {
        private readonly IService _service;
        private ObservableCollection<string> _games;

        public GamesViewModel(IService service)
        {
            _service = service;
            Games = new ObservableCollection<string>(_service.GetGames());
            Messenger.Default.Register<GameAdded>(this, AddGame);
        }

        private void AddGame(GameAdded obj)
        {
            //_games.Add(obj.Game);
        }

        public ObservableCollection<string> Games
        {
            get { return _games; }
            set
            {
                Set(ref _games, value);
            }
        }
    }
    
}