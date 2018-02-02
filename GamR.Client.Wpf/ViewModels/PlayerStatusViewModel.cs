using GalaSoft.MvvmLight;

namespace GamR.Client.Wpf.ViewModels
{
    public class PlayerStatusViewModel : ViewModelBase
    {
        private decimal _totalScore;

        public PlayerStatusViewModel(string name, decimal totalScore)
        {
            Name = name;
            TotalScore = totalScore;
        }

        public string Name { get; }

        public decimal TotalScore
        {
            get { return _totalScore; }
            set { Set(ref _totalScore, value); }
        }
    }
}