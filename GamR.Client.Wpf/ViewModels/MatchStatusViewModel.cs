using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GamR.Client.Wpf.Events;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.ViewModels.Interfaces;

namespace GamR.Client.Wpf.ViewModels
{
    public class MatchStatusViewModel : ViewModelBase,IMatchStatusViewModel
    {
        private readonly IService _service;
        private ObservableCollection<PlayerStatusViewModel> _playerStatusViewModels;

        public MatchStatusViewModel(IService service)
        {
            _service = service;
            Messenger.Default.Register<GameAdded>(this,AddGame);
            Task.Run(async () => await UpdatePlayerStatusCollection());
        }

        private async Task UpdatePlayerStatusCollection()
        {
            PlayerStatusViewModels = new ObservableCollection<PlayerStatusViewModel>(await _service.GetStatusses());
        }

        public ObservableCollection<PlayerStatusViewModel> PlayerStatusViewModels
        {
            get { return _playerStatusViewModels; }
            set { Set(ref _playerStatusViewModels, value); }
        }

        private void AddGame(GameAdded obj)
        {
            //var playerStatus = PlayerStatusViewModels.SingleOrDefault(ps => ps.Name == obj.Game.Melder);
            //if (playerStatus == null) return;
            //playerStatus.TotalScore += obj.Game.Result;
        }
    }

    public class PlayerStatusViewModel : ViewModelBase
    {
        private decimal _totalScore;

        public PlayerStatusViewModel(string name, decimal totalScore)
        {
            Name = name;
            TotalScore = totalScore;
        }

        public string Name { get; }

        public decimal TotalScore
        {
            get { return _totalScore; }
            set { Set(ref _totalScore, value); }
        }
    }
}