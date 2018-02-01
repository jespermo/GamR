using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GamR.Client.Wpf.Events;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.ViewModels.Interfaces;

namespace GamR.Client.Wpf.ViewModels
{
    public class GamesViewModel : ViewModelBase, IGamesViewModel
    {
        private readonly IService _service;
        private ObservableCollection<string> _games;

        public GamesViewModel(IService service)
        {
            _service = service;
            Task.Run(async ()=> await UpdateGamesCollection());
            Messenger.Default.Register<GameAdded>(this, AddGame);
        }

        private async Task UpdateGamesCollection()
        {
            Games = new ObservableCollection<string>(await _service.GetGames());
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