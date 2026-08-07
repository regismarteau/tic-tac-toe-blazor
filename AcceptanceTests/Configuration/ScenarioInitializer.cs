using AcceptanceTests.Extensions;
using Reqnroll;

namespace AcceptanceTests.Configuration
{
    [Binding]
    public class ScenarioInitializer
    {
        [BeforeScenario]
        public static void Init(ScenarioContext context)
        {
            context.Set(new TestServices(context));
        }

        [AfterScenario]
        public static void Clean(ScenarioContext context)
        {
            context.GetTestServices().Dispose();
        }
    }
}
