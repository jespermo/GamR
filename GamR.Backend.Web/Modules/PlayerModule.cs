﻿using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class PlayerModule : NancyModule
    {

        private static List<Player> _players;

        public PlayerModule()
        {
            CreatePlayers();
            Get("/players", args =>
                            {
                                var response =  Response.AsJson<List<Player>>(_players);
                                response.Headers.Add("Content-Type","application/json");
                                return response;
                            });
            Get("/player/{id}", args =>
                                {
                                    return Enumerable.SingleOrDefault<Player>(_players, p => p.Id == args.id);
                                });

            Put("/player", _ =>
                           {
                               var request = this.Bind<Player>();
                               _players.RemoveAll(x => x.Id == request.Id);
                               _players.Add(request);
                               return Response.AsJson<Player>(request);
                           });
        }


        private void CreatePlayers()
        {
            _players = _players ?? new List<Player>
                                   {
                                       new Player
                                       {
                                           Id = 1,
                                           FirstName = "John",
                                           LastName = "Tolkien",
                                           Email = "tolkien@inklings.com",
                                           PhoneNumber = "867-5309"
                                       },
                                       new Player
                                       {
                                           Id = 4,
                                           FirstName = "Arne",
                                           LastName = "Jensen",
                                           Email = "Arne@Jensen.com",
                                           PhoneNumber = "867-5sa309"
                                       },
                                       new Player
                                       {
                                           Id = 3,
                                           FirstName = "Birger",
                                           LastName = "Bo",
                                           Email = "bb@inklings.com",
                                           PhoneNumber = "81167-5309"
                                       },
                                   };
        
        }
    }

    class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}