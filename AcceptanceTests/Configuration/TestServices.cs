using Bunit;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Reqnroll;
using Web;

namespace AcceptanceTests.Configuration;

public class TestServices : BunitContext
{
    public TestServices(ScenarioContext context)
    {
        Services
            .AddTicTacToeServices(new ConfigurationBuilder().Build())
            .AddWebServices()
            .SubstituteServices(context);
    }
}
