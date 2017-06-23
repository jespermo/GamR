using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;

namespace GamR.Modules
{
    public class TestModule : NancyModule
    {
        public TestModule(Startup.ITest test)
        {
            var testet = test;
            Get("/", args => "Hej dut");
        }
    }
}
