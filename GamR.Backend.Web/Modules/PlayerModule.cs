using System;
using System.Linq;
using GamR.Backend.Web.Views;
using Nancy;

namespace GamR.Backend.Web.Modules
{
    public class PlayerModule : NancyModule
    {


        public PlayerModule(PlayersViewManager playersViewManager)
        {
            Get("/players", args =>
            {
                var response = Response.AsJson(playersViewManager.Players
                    .Select(x => new Player {Name = x.Value, Id = x.Key}).ToList());
                                response.Headers.Add("Content-Type","application/json");
                                return response;
                            });
        }


    }

    class Player
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

    }
}