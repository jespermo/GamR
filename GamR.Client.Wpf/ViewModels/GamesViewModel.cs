using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GamR.Client.Wpf.Events;
using GamR.Client.Wpf.Models;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.ViewModels.Interfaces;

namespace GamR.Client.Wpf.ViewModels
{
    public class GamesViewModel : ViewModelBase, IGamesViewModel
    {
        private readonly IService _service;
        private ObservableCollection<string> _games;

        public GamesViewModel(IService service)
        {
            _service = service;
            Messenger.Default.Register<GameAdded>(this, AddGame);

            MessengerInstance.Register<PropertyChangedMessage<Match>>(this, m =>
            {
                var newValueId = m.NewValue?.Id;
                if (newValueId == null)
                {
                    Games = new ObservableCollection<string>();
                }
                Task.Run(async () =>
                {
                    await UpdateGamesCollection(newValueId.Value);
                });
            });
        }
        

        private async Task UpdateGamesCollection(Guid matchId)
        {
            var games = await _service.GetGames(matchId);
            Games = new ObservableCollection<string>(games.Select(c=>
            {
                var meldingPlayers = string.Join(",",c.MeldingPlayers ?? new List<string>());
                return $"{meldingPlayers}-{c.Melding}-{c.NumberOfTrics}-{c.ActualNumberOfTricks}";
            }));
        }

        private void AddGame(GameAdded obj)
        {
            //_games.Add(obj.Game);
        }

        public ObservableCollection<string> Games
        {
            get { return _games; }
            set
            {
                Set(ref _games, value);
            }
        }
    }
    
}