using WpfApp1.ViewModels;

namespace WpfApp1
{
    public class Game : IGame
    {
        public Game(string melder, string melding, int trumps, decimal result)
        {
            Melder = melder;
            Melding = melding;
            Trumps = trumps;
            Result = result;
        }

        public string Melder { get; }
        public string Melding { get; }
        public int Trumps { get; }
        public decimal Result { get; }
    }
}