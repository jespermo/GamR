using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GamR.Client.Wpf.Events;
using GamR.Client.Wpf.Models;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.Util;
using WpfApp1.Annotations;

namespace GamR.Client.Wpf.ViewModels
{
    public class NewMatchViewModel : ViewModelBase
    {
        private DateTime _date;
        private string _location;
        private IService _service;

        public NewMatchViewModel([NotNull] IService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            CreateMatchCommand = new AsyncDelegateCommand<Window>(Save);
            CancelCommand = new RelayCommand<Window>(Cancel);
            Date = DateTime.Now;
        }

        public ICommand CancelCommand { get; set; }

        public ICommand CreateMatchCommand { get; set; }

        private async Task Save(Window arg)
        {
            try
            {
                var matchId = await _service.CreateMatch(new NewMatch { Date = Date, Location = Location });
                MessengerInstance.Send(new MatchCreated(matchId));
                arg?.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

        private void Cancel(Window arg)
        {
            arg?.Close();
        }
    }
}