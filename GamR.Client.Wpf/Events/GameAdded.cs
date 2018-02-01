using GamR.Client.Wpf.Models;

namespace GamR.Client.Wpf.Events
{
    public class GameAdded
    {
        public Game Game { get; }

        public GameAdded(Game game)
        {
            Game = game;
        }
    }
}