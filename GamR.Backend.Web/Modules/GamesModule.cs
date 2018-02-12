﻿using System;
 using System.Dynamic;
 using System.Linq;
 using System.Threading;
using GamR.Backend.Core.Framework;
 using GamR.Backend.Web.Views;
 using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nancy;
 using Nancy.ModelBinding;

namespace GamR.Backend.Web.Modules
{
    public class GamesModule : NancyModule
    {
        public GamesModule()
        {
            //Get("/Games", _ =>
            //{
            //    var games = matches.MatchesView.Select(g => $"{g.Value.Player1Score} {g.Value.Player2Score} {g.Value.Player3Score} {g.Value.Player4Score}");
            //    return Response.AsJson(games);
            //});
            Post("/Game", _ =>
            {
                CreateGame request = this.Bind<CreateGame>();

                return Response.AsJson($"OK: {request.Melder}-{request.Melding}-{request.Trumps}-{request.Result}");
            });
        }

        class CreateGame
        {
            public string Melder { get; }
            public string Melding { get; }
            public int Trumps { get; }
            public decimal Result { get; }
        }
    }
}

