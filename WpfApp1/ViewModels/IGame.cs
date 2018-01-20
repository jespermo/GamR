namespace WpfApp1.ViewModels
{
    public interface IGame
    {
        string Melder { get; }
        string Melding { get; }
        int Trumps { get;  }
        decimal Result { get; }
    }
}