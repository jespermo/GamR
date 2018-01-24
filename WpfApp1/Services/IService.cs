using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using WpfApp1.ViewModels;

namespace WpfApp1.Services
{
    public interface IService
    {
        List<string> GetGames();
        void AddNewGame(Game game);
        List<PlayerStatusViewModel> GetStatusses();
    }

    public class Service : IService
    {
        private readonly IRequester _requester;

        public Service(IRequester requester)
        {
            _requester = requester;
            Games = Task.Run(async () => await _requester.Get<List<string>>("/Games")).Result;
        }
        private List<string> Games { get; }

    public List<string> GetGames()
        {
            return Games;
        }

        public void AddNewGame(Game game)
        {
            //games.Add(game);
            Messenger.Default.Send(new GameAdded(game));
        }

        public List<PlayerStatusViewModel> GetStatusses()
        {

            return new List<PlayerStatusViewModel>();//games.GroupBy(g=>g.GameInfo.Split(' ')[0]).Select(x=>new PlayerStatusViewModel(x.Key,2)).ToList());
        }
    }

    public class GameAdded
    {
        public Game Game { get; }

        public GameAdded(Game game)
        {
            Game = game;
        }
    }
}