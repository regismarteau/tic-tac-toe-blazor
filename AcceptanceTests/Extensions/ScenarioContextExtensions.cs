using AcceptanceTests.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using RMediator.Abstractions;

namespace AcceptanceTests.Extensions
{
    public static class ScenarioContextExtensions
    {
        extension(ScenarioContext context)
        {
            public async Task WaitForSideEffects()
            {
                await context.GetService<AsynchronousSideEffectsAwaiter>().WaitForSideEffects();
            }

            public T GetService<T>() where T : notnull
            {
                return context.GetTestServices().Services.GetRequiredService<T>();
            }

            public TestServices GetTestServices()
            {
                return context.Get<TestServices>();
            }
        }
    }
}
