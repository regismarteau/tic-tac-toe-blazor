using AcceptanceTests.ErrorHandling;
using Reqnroll;
using RMediator.Abstractions;

namespace AcceptanceTests.Configuration
{
    public static class ScenarioContextExtensions
    {
        extension(ScenarioContext context)
        {
            public async Task Dispatch(ICommand command)
            {
                await context.GetService<IDispatchCommand>().Dispatch(command);
                await context.GetService<AsynchronousSideEffectsAwaiter>().WaitForSideEffects();
            }

            public async Task<T> Dispatch<T>(ICommand<T> command)
            {
                var result = await context.GetService<IDispatchCommand>().Dispatch(command);
                await context.GetService<AsynchronousSideEffectsAwaiter>().WaitForSideEffects();
                return result;
            }

            public Task<T> Dispatch<T>(IQuery<T> query)
            {
                return context.GetService<IDispatchQuery>().Dispatch(query);
            }

            public T GetService<T>() where T : notnull
            {
                return context.GetTestServices().GetService<T>();
            }

            public TestServices GetTestServices()
            {
                return context.Get<TestServices>();
            }
        }
    }
}
