using AcceptanceTests.Extensions;
using Reqnroll;

namespace AcceptanceTests.Configuration;

[Binding]
public class ScenarioInitializer
{
    [BeforeScenario]
    public static void Init(ScenarioContext context) => context.Set(new TestServices(context));

    [AfterScenario]
    public static async Task Clean(ScenarioContext context) => await context.GetTestServices().DisposeAsync();
}
