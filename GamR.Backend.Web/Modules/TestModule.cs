﻿using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using GamR.Backend.Core.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nancy;
using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class GamesModule : NancyModule
    {
        public GamesModule()
        {
            
        }
    }
    public class PlayerModule : NancyModule
    {
        private readonly PlayersView _playersView;

        public PlayerModule(PlayersView playersView)
        {
            _playersView = playersView;
            Get("/players", args =>
                            {
                                var response = Response.AsJson(_playersView.Players);
                                response.Headers.Add("Content-Type", "application/json");
                                return response;
                            });
            //Get("/player/{id}", args =>
            //                    {
            //                        return _players.SingleOrDefault(p => p.Id == args.id);
            //                    });

            //Put("/player", _ =>
            //               {
            //                   var request = this.Bind<Player>();
            //                   _players.RemoveAll(x => x.Id == request.Id);
            //                   _players.Add(request);
            //                   return Response.AsJson(request);
            //               });
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

