using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamR.Client.Wpf.Models;
using GamR.Client.Wpf.ViewModels;
using WpfApp1;

namespace GamR.Client.Wpf.Services
{
    public interface IService
    {
        Task<List<Game>> GetGames(Guid matchId);
        Task AddNewGame(Game game, Guid matchId);
        Task<MatchStatus> GetStatusses(Guid matchId);
        Task<Guid> CreateMatch(NewMatch newMatch);
        Task<List<Match>> GetMatches();
        Task<List<Player>> GetPlayers();
    }
}