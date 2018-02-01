using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GamR.Client.Wpf.Events;
using GamR.Client.Wpf.Models;
using GamR.Client.Wpf.ViewModels;
using WpfApp1;

namespace GamR.Client.Wpf.Services
{
    public class Service : IService
    {
        private readonly IRequester _requester;

        public Service(IRequester requester)
        {
            _requester = requester;

            Games = Task.Run(async () => await _requester.Get<List<string>>("/Games")).GetAwaiter().GetResult();
        }
        private List<string> Games { get; }

        public Task<List<string>> GetGames()
        {
            return Task.FromResult(Games);
        }

        public Task AddNewGame(Game game)
        {
            Games.Add(game.GameInfo);
            Messenger.Default.Send(new GameAdded(game));
            return Task.CompletedTask;
        }

        public  Task<List<PlayerStatusViewModel>> GetStatusses()
        {

            return Task.FromResult(new List<PlayerStatusViewModel>());//games.GroupBy(g=>g.GameInfo.Split(' ')[0]).Select(x=>new PlayerStatusViewModel(x.Key,2)).ToList());
        }
    }
}