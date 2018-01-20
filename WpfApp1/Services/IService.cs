using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using WpfApp1.ViewModels;

namespace WpfApp1.Services
{
    public interface IService
    {
        List<IGame> GetGames();
        void AddNewGame(Game game);
        List<PlayerStatusViewModel> GetStatusses();
    }

    public class Service : IService
    {
        private static List<IGame> games  = new List<IGame>
        {
            new Game("Søren", "10 KLØR",11,4),
            new Game("Asbjørn", "SOL",2,-6),
            new Game("Jesper", "11 KLØR",11,4),
            new Game("Baastrup", "9 KLØR",11,4),
            new Game("Søren", "10 VIP",11,8)
        };

    public List<IGame> GetGames()
        {
            return games;
        }

        public void AddNewGame(Game game)
        {
            games.Add(game);
            Messenger.Default.Send(new GameAdded(game));
        }

        public List<PlayerStatusViewModel> GetStatusses()
        {
            
            return new List<PlayerStatusViewModel>(games.GroupBy(g=>g.Melder).Select(x=>new PlayerStatusViewModel(x.Key,x.Sum(p=>p.Result))).ToList());
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