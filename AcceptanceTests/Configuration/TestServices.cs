using Bunit;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Reqnroll;
using Web.Configurations;

namespace AcceptanceTests.Configuration;

public class TestServices : BunitContext
{
    public TestServices(ScenarioContext context)
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services
            .AddTicTacToeServices(new ConfigurationBuilder().Build())
            .AddWebServices()
            .SubstituteServices(context);
    }
}
