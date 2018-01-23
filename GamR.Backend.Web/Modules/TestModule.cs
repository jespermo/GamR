﻿using System;
 using System.Dynamic;
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
            Get("/Test", _ => Response.AsJson($"{DateTime.Now}"));
            Post("/Game", _ =>
            {
                TestGame request = this.Bind<TestGame>();

                return Response.AsJson($"OK: {request.Melder}-{request.Melding}-{request.Trumps}-{request.Result}");
            });
        }

        class TestGame
        {
            public string Melder { get; }
            public string Melding { get; }
            public int Trumps { get; }
            public decimal Result { get; }
        }
    }
}

