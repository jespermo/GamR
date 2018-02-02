using System.Windows;
using Autofac;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.ViewModels;
using GamR.Client.Wpf.ViewModels.Interfaces;

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
            var container = CreateContainer();
            DataContext = container.Resolve<MainViewModel>();
        }

        private IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterType<MainViewModel>();
            containerBuilder
                .RegisterType<GamesViewModel>()
                .As<IGamesViewModel>();
            containerBuilder
                .RegisterType<MatchStatusViewModel>()
                .As<IMatchStatusViewModel>();
            containerBuilder
                .RegisterType<NewGameViewModel>();
            containerBuilder
                .RegisterType<NewMatchViewModel>();

            containerBuilder
                .RegisterType<Service>()
                .As<IService>();

            containerBuilder
                .RegisterType<Requester>()
                .As<IRequester>();





            return containerBuilder.Build();
        }
    }
}
