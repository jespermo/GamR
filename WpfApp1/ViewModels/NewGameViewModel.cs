﻿using System.ComponentModel;
using System.Threading.Tasks;
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
        private readonly IRequester _requester;
        private string _melder;
        private string _melding;
        private int _trumps;
        private decimal _result;

        public NewGameViewModel(IService service, IRequester requester)
        {
            _service = service;
            _requester = requester;
            SaveGameCommand = new AsyncDelegateCommand<Window>(Save);
            CancelCommand = new RelayCommand<Window>(Cancel);
            PropertyChanged += RaiseCanExecute;
        }

        private void RaiseCanExecute(object sender, PropertyChangedEventArgs e)
        {
            //TODO
        }

        public ICommand CancelCommand { get; set; }

        private void Cancel(Window obj)
        {
            obj.Close();
        }


        public ICommand SaveGameCommand { get; set; }

        private bool CanSave(Window obj)
        {
            return !string.IsNullOrEmpty(Melder) && !string.IsNullOrEmpty(Melding) && _trumps >= 0 && Result != 0;
        }

        private async Task Save(Window obj)
        {
            //var game = new Game(Melder, Melding, Trumps, Result);
            //_service.AddNewGame(game);
            //var res = await _requester.Post<string>(game,"Game");
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