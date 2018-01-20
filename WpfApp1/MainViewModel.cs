using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpfApp1.Services;
using WpfApp1.ViewModels;
using WpfApp1.Views;

namespace WpfApp1
{
    public class MainViewModel : ViewModelBase
    {
        private string _title;
        private IGamesViewModel _gamesViewModel;
        private IService _service;

        public MainViewModel(IService service)
        {
            _service = service;
            Title = "GAMR";
            GamesViewModel = new GamesViewModel(service);
            MatchStatusViewModel = new MatchStatusViewModel(service);
            AddNewGame = new RelayCommand(CreateNewGame);
        }

        public IMatchStatusViewModel MatchStatusViewModel { get; set; }

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