using System.Windows;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.ViewModels;

namespace GamR.Client.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var requester = new Requester();
            DataContext = new MainViewModel(new Service(requester), requester);
        }
    }
}
