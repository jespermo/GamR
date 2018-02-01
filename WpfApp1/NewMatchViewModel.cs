using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpfApp1.Services;

namespace WpfApp1
{
    public class NewMatchViewModel : ViewModelBase
    {
        private readonly IRequester _requester;
        private DateTime _date;
        private string _location;

        public NewMatchViewModel(IRequester requester)
        {
            _requester = requester;
            CreateMatchCommand = new AsyncDelegateCommand<Window>(Save);
            CancelCommand = new RelayCommand<Window>(Cancel);
            Date = DateTime.Now;
        }

        public ICommand CancelCommand { get; set; }

        public ICommand CreateMatchCommand { get; set; }

        private async Task Save(Window arg)
        {
            await _requester.Post<NewMatch>(new NewMatch {Date = Date, Location = Location}, "/Match");
            MessengerInstance.Send(new MatchCreated());
            arg?.Close();
        }

        public string Location
        {
            get { return _location; }
            set { Set(ref _location, value); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        class NewMatch
        {
            public DateTime Date { get; set; }
            public string Location { get; set; }
        }

        private void Cancel(Window arg)
        {
            arg?.Close();
        }
    }

    public  class MatchCreated
    {
    }
}