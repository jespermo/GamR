using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GamR.Client.Wpf.Events;
using GamR.Client.Wpf.Models;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.ViewModels.Interfaces;

namespace GamR.Client.Wpf.ViewModels
{
    public class MatchStatusViewModel : ViewModelBase,IMatchStatusViewModel
    {
        private readonly IService _service;
        private MatchStatus _matchStatus;
        private Guid _matchId;

        public MatchStatusViewModel(IService service)
        {
            _service = service;
            Messenger.Default.Register<GameAdded>(this,Update);
            MessengerInstance.Register<PropertyChangedMessage<Match>>(this, m =>
            {

                var newValue = m.NewValue?.Id;
                if (newValue == null)
                {
                    MatchStatus = null;
                }
                else
                {
                    Task.Run(async () =>
                    {
                        _matchId = newValue.Value;
                        MatchStatus = await _service.GetStatusses(_matchId);
                    });
                }
            });
        }

        private void Update(GameAdded obj)
        {
            Task.Run(async () => MatchStatus = await _service.GetStatusses(_matchId));
        }

        public MatchStatus MatchStatus
        {
            get { return _matchStatus; }
            set { Set(ref _matchStatus, value); }
        }
        
    }
}