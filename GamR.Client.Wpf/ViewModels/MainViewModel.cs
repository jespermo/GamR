using System;
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

        public MainViewModel([NotNull] IService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            Title = "GAMR";
            GamesViewModel = new GamesViewModel(service);
            MatchStatusViewModel = new MatchStatusViewModel(service);
            AddNewGame = new RelayCommand(CreateNewGame);
            CreateMatchCommand = new RelayCommand(CreateMatch);
            Task.Run(async () => await UpdateMatches());
            Messenger.Default.Register<MatchCreated>(this, match => Task.Run(async () => await UpdateMatches()));
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
            newGameDialog.DataContext = new NewGameViewModel(_service);
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