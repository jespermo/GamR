using System;
using System.Threading.Tasks;
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
        private readonly IRequester _requester;
        private string _text;

        public MainViewModel(IService service, IRequester requester)
        {
            _service = service;
            _requester = requester;
            Title = "GAMR";
            GamesViewModel = new GamesViewModel(service);
            MatchStatusViewModel = new MatchStatusViewModel(service);
            AddNewGame = new RelayCommand(CreateNewGame);
            TestCommand = new AsyncDelegateCommand<object>(UpdateTestData);
        }

        private async Task UpdateTestData(object arg)
        {
            var data = await _requester.Get<string>("test");
            if (DateTime.TryParse(data, out var parsedData))
            {
                Text = parsedData.ToString("HH:mm:ss tt zz");
            }
            else
            {
                Text = $"Error:{data.Substring(data.Length - 1, data.Length)}";
            }
        }

        public ICommand TestCommand { get; set; }
        

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public IMatchStatusViewModel MatchStatusViewModel { get; set; }

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