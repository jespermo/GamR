using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WpfApp1.Annotations;

namespace GamR.Client.Wpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _title;
        private IGamesViewModel _gamesViewModel;
        private readonly IService _service;
        private string _text;
        private ObservableCollection<Match> _matches;
        private Match _selectedMatch;

        public MainViewModel(
            [NotNull] IService service, 
            [NotNull] IGamesViewModel gamesViewModel,
            [NotNull] IMatchStatusViewModel matchStatusViewModel)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            GamesViewModel = gamesViewModel ?? throw new ArgumentNullException(nameof(gamesViewModel));
            MatchStatusViewModel = matchStatusViewModel ?? throw new ArgumentNullException(nameof(matchStatusViewModel));
            AddNewGame = new RelayCommand(CreateNewGame, CanCreateNewGame);
            CreateMatchCommand = new RelayCommand(CreateMatch);
            Task.Run(async () => await UpdateMatches());
            Messenger.Default.Register<MatchCreated>(this, match => Task.Run(async () => await UpdateMatches()));
        }

        private bool CanCreateNewGame()
        {
            return _selectedMatch != null;
        }


        public Match SelectedMatch
        {
            get { return _selectedMatch; }
            set
            {
                if (Set(ref _selectedMatch, value, broadcast: true))
                {
                    AddNewGame.RaiseCanExecuteChanged();
                }
            }
        }

        private async Task UpdateMatches()
        {
            var matches = await _service.GetMatches();
            Matches = new ObservableCollection<Match>(matches);
        }

        private void CreateMatch()
        {
            var newGameDialog = new NewMatchDialog {DataContext = new NewMatchViewModel(_service)};
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
            newGameDialog.DataContext = new NewGameViewModel(_service, _selectedMatch.Id);
            newGameDialog.Show();
        }

        public string Title { get; } = "GAMR";

        public IGamesViewModel GamesViewModel
        {
            get { return _gamesViewModel; }
            set
            {
                Set(ref _gamesViewModel,  value);
            }
        }

        public RelayCommand AddNewGame { get; set; }
    }
}