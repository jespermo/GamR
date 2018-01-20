using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class NewGameViewModel : ViewModelBase
    {
        private readonly IService _service;
        private string _melder;
        private string _melding;
        private int _trumps;
        private decimal _result;

        public NewGameViewModel(IService service)
        {
            _service = service;
            SaveGameCommand = new RelayCommand<Window>(Save, CanSave);
            CancelCommand = new RelayCommand<Window>(Cancel);
            PropertyChanged += RaiseCanExecute;
        }

        private void RaiseCanExecute(object sender, PropertyChangedEventArgs e)
        {
            SaveGameCommand.RaiseCanExecuteChanged();
        }

        public ICommand CancelCommand { get; set; }

        private void Cancel(Window obj)
        {
            obj.Close();
        }


        public RelayCommand<Window> SaveGameCommand { get; set; }

        private bool CanSave(Window obj)
        {
            return !string.IsNullOrEmpty(Melder) && !string.IsNullOrEmpty(Melding) && _trumps >= 0 && Result != 0;
        }

        private void Save(Window obj)
        {
            _service.AddNewGame(new Game(Melder, Melding, Trumps, Result));
            obj?.Close();
        }

        public string Melder
        {
            get
            {
                return _melder;
            }
            set
            {
                Set(ref _melder, value);
            }
        }

        public string Melding
        {
            get
            {
                return _melding;
            }
            set
            {
                Set(ref _melding, value);
            }
        }

        public int Trumps
        {
            get
            {
                return _trumps;
            }
            set
            {
                Set(ref _trumps, value);
            }
        }

        public decimal Result
        {
            get
            {
                return _result;
            }
            set
            {
                Set(ref _result, value);
            }
        }
    }
}