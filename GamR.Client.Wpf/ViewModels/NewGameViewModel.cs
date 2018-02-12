using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GamR.Client.Wpf.Models;
using GamR.Client.Wpf.Services;
using GamR.Client.Wpf.Util;

namespace GamR.Client.Wpf.ViewModels
{
    public class NewGameViewModel : ViewModelBase
    {
        private readonly IService _service;
        private readonly Guid _matchId;
        private Dictionary<string, object> _melders;
        private string _melding;
        private int _meldingTrumps;
        private int _actualTricks;
        private string _vip;
        private Dictionary<string, object> _meldingTeam;
        private Dictionary<string, object> _players;
        private ObservableCollection<string> _meldings;
        private Visibility _notNormalResultVisibility;
        private Visibility _normalResultVisibility;
        private ObservableCollection<SolPlayerViewModel> _playersMinimizingTricks;
        private string[] _minimizingMeldings;
        private string[] _normalMeldings;

        public NewGameViewModel(IService service, Guid matchId)
        {
            _service = service;
            _matchId = matchId;
            SaveGameCommand = new AsyncDelegateCommand<Window>(Save);
            CancelCommand = new RelayCommand<Window>(Cancel);

            var players = Task.Run(async () => await _service.GetPlayers()).GetAwaiter().GetResult();
            var dictionary = players.ToDictionary(p => p.Name, p1 => (object)p1);
            PlayersMinimizingTricks = new ObservableCollection<SolPlayerViewModel>();
            Melders = new Dictionary<string,object>();
            Players = new Dictionary<string, object>(dictionary);
            MeldingTeam = new Dictionary<string, object>();
            SetupMeldings();
            PropertyChanged += RaiseCanExecute;
        }

        private void SetupMeldings()
        {
            _minimizingMeldings = new[] {"SOL", "REN SOL", "OPLÆGGER SOL"};
            _normalMeldings = new[] {
                "GRAN",
                "KLØR",
                "TRUMFFRI",
                "VIP",
                "SPAR",
                "HJERTER",
                "RUDER"
            };

            Meldings = new ObservableCollection<string>(_minimizingMeldings.Concat(_normalMeldings));
        }

        private void RaiseCanExecute(object sender, PropertyChangedEventArgs e)
        {
            //TODO
        }

        public ICommand CancelCommand { get; set; }

        private void Cancel(Window obj)
        {
            obj.Close();
        }


        public ICommand SaveGameCommand { get; set; }
        
        private async Task Save(Window obj)
        {
            var game = new Game
            {
                Melding = Melding,
                Players = Players.Values.Cast<Player>().Select(p => new PlayerScore{Id = p.Id,Name = p.Name}).ToList(),
                MeldingPlayers = Melders.Keys.ToList(),
                MeldingTeam = MeldingTeam.Values.Cast<Player>().Select(p => new PlayerScore { Id = p.Id, Name = p.Name }).ToList(),
                TeamTricks = CreateTeamTricks(),
                NumberOfVips = Vip,
                NumberOfTricks = MeldingTrumps
            };
            await _service.AddNewGame(game, _matchId);
            obj?.Close();
        }

        private List<TeamTricks> CreateTeamTricks()
        {
            if (NormalResultVisibility == Visibility.Visible)
            {
                return new List<TeamTricks>{new TeamTricks{TeamId = ((Player) Melders.First().Value).Id,Result = ActualTricks}};
            }
            else
            {
                return PlayersMinimizingTricks.Select(p => new TeamTricks {TeamId = p.Id, Result = p.PlayerActualTricks})
                    .ToList();

            }
        }

        public Dictionary<string, object> Melders
        {
            get
            {
                return _melders;
            }
            set
            {
                Set(ref _melders, value);
            }
        }

        public string Melding
        {
            get
            {
                return _melding;
            }
            set
            {
                if (Set(ref _melding, value))
                {
                    UpdateMinimizingPlayers();
                }
            }
        }

        private void UpdateMinimizingPlayers()
        {
            PlayersMinimizingTricks.Clear();
            if (Melding?.EndsWith("SOL") == true)
            {
                SetMinimizingPlayersVisibility(true);
                Melders.Values.Cast<Player>().ToList().ForEach(p => PlayersMinimizingTricks.Add(new SolPlayerViewModel{Name = p.Name,Id = p.Id}));
                return;
            }
            SetMinimizingPlayersVisibility(false);

        }

        public int MeldingTrumps
        {
            get
            {
                return _meldingTrumps;
            }
            set
            {
                Set(ref _meldingTrumps, value);
            }
        }

        public int ActualTricks
        {
            get
            {
                return _actualTricks;
            }
            set
            {
                Set(ref _actualTricks, value);
            }
        }

        public string Vip
        {
            get { return _vip; }
            set { Set(ref _vip, value); }
        }

        public Dictionary<string, object> MeldingTeam
        {
            get { return _meldingTeam; }
            set { Set(ref _meldingTeam, value); }
        }

        public ObservableCollection<string> Meldings
        {
            get { return _meldings; }
            set { Set(ref _meldings, value); }
        }

        public Dictionary<string, object> Players
        {
            get { return _players; }
            set { Set(ref _players, value); }
        }

        private void SetMinimizingPlayersVisibility(bool minimizingVisible)
        {
            NormalResultVisibility = minimizingVisible ? Visibility.Collapsed : Visibility.Visible;
            NotNormalResultVisibility = minimizingVisible ? Visibility.Visible: Visibility.Collapsed;
        }
        public Visibility NotNormalResultVisibility
        {
            get { return _notNormalResultVisibility; }
            set { Set(ref _notNormalResultVisibility, value); }
        }

        public Visibility NormalResultVisibility
        {
            get { return _normalResultVisibility; }
            set { Set(ref _normalResultVisibility, value); }
        }

        public ObservableCollection<SolPlayerViewModel> PlayersMinimizingTricks
        {
            get { return _playersMinimizingTricks; }
            set { Set(ref _playersMinimizingTricks, value); }
        }
    }

    public class SolPlayerViewModel : ViewModelBase
    {
        private string _name;
        private int _playerActualTricks;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public int PlayerActualTricks
        {
            get { return _playerActualTricks; }
            set { Set(ref _playerActualTricks, value); }
        }

        public Guid Id { get; set; }
    }
}