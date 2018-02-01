using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GamR.Client.Wpf.Events;
using GamR.Client.Wpf.Models;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.ViewModels.Interfaces;
using GamR.Client.Wpf.Views;

namespace GamR.Client.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _title;
        private IGamesViewModel _gamesViewModel;
        private readonly IService _service;
        private readonly IRequester _requester;
        private string _text;
        private ObservableCollection<Match> _matches;

        public MainViewModel(IService service, IRequester requester)
        {
            _service = service;
            _requester = requester;
            Title = "GAMR";
            GamesViewModel = new GamesViewModel(service);
            MatchStatusViewModel = new MatchStatusViewModel(service);
            AddNewGame = new RelayCommand(CreateNewGame);
            CreateMatchCommand = new RelayCommand(CreateMatch);
            Task.Run(async () => await UpdateMatches());
            Messenger.Default.Register<MatchCreated>(this, Update);
        }

        private void Update(MatchCreated obj)
        {
            Task.Run(async ()=>await UpdateMatches());
        }

        private async Task UpdateMatches()
        {
            var matches = await _requester.Get<List<Match>>("/matchs");
            Matches = new ObservableCollection<Match>(matches);
        }

        private void CreateMatch()
        {
            var newGameDialog = new NewMatchDialog {DataContext = new NewMatchViewModel(_requester)};
            newGameDialog.Show();
        }

        public ICommand CreateMatchCommand { get; set; }
        

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public IMatchStatusViewModel MatchStatusViewModel { get; set; }

        public ObservableCollection<Match> Matches
        {
            get { return _matches; }
            set { Set(ref _matches, value); }
        }

        private void CreateNewGame()
        {
            var newGameDialog = new NewGameDialog();
            newGameDialog.DataContext = new NewGameViewModel(_service, _requester);
            newGameDialog.Show();
        }

        public string Title
        {
            get { return _title; }
            private set
            {
                _title = value;
            }
        }

        public IGamesViewModel GamesViewModel
        {
            get { return _gamesViewModel; }
            set
            {
                _gamesViewModel = value;
                
            }
        }

        public ICommand AddNewGame { get; set; }
    }
}