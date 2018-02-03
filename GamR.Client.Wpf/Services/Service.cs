using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GamR.Client.Wpf.Events;
using GamR.Client.Wpf.Models;
using GamR.Client.Wpf.ViewModels;

namespace GamR.Client.Wpf.Services
{
    public class Service : IService
    {
        private readonly IRequester _requester;

        public Service(IRequester requester)
        {
            _requester = requester;

        }

        public async Task<List<Game>> GetGames(Guid matchId)
        {
            return await _requester.Get<List<Game>>($"/match/{matchId}/games");
        }

        public async Task AddNewGame(Game game)
        {
            await _requester.Post<bool>(game, "/game");
            Messenger.Default.Send(new GameAdded(game));
        }

        public  Task<List<PlayerStatusViewModel>> GetStatusses()
        {

            return Task.FromResult(new List<PlayerStatusViewModel>());//games.GroupBy(g=>g.GameInfo.Split(' ')[0]).Select(x=>new PlayerStatusViewModel(x.Key,2)).ToList());
        }

        public async Task<Guid> CreateMatch(NewMatch newMatch)
        {
            return await _requester.Post<Guid>(newMatch, "/match");
        }

        public async Task<List<Match>> GetMatches()
        {
            return await _requester.Get<List<Match>>("/matches");
        }
    }
}