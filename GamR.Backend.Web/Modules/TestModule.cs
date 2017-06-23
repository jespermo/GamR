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
}
