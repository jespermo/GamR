using WpfApp1.ViewModels;

namespace WpfApp1
{
    public class Game : IGame
    {
        public Game(string gameInfo)
        {
            GameInfo = gameInfo;
        }
        public string GameInfo { get; }
    }
}