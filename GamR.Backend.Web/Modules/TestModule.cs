using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nancy;

namespace GamR.Backend.Web.Modules
{
    public class TestModule : NancyModule
    {
        public TestModule(Startup.ITest test)
        {
            var testet = test;
            Get("/", args => "Hej dut");
        }
    }

    public class PlayerModule : NancyModule
    {

        private List<Player> _players;

        public PlayerModule()
        {
            CreatePlayers();
            Get("/players", args =>
                            {
                                var response =  Response.AsJson(_players);
                                response.Headers.Add("Content-Type","application/json");
                                return response;
                            });
        }


        private void CreatePlayers()
        {
            _players = new List<Player>
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

