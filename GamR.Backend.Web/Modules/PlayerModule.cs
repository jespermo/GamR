using System;
using System.Collections.Generic;
using System.Linq;
using GamR.Backend.Web.Views;
using Nancy;
using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class PlayerModule : NancyModule
    {

        private static List<Player> _players;

        public PlayerModule(ViewContainer viewContainer)
        {
            Get("/players", args =>
            {
                var response = Response.AsJson(viewContainer.PlayersView.Players
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