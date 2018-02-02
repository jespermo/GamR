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
        Task<List<string>> GetGames(Guid matchId);
        Task AddNewGame(Game game);
        Task<List<PlayerStatusViewModel>> GetStatusses();
        Task<Guid> CreateMatch(NewMatch newMatch);
        Task<List<Match>> GetMatches();
    }
}