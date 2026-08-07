using AcceptanceTests.Extensions;
using Infrastructure.OutboxServices;
using Reqnroll;

namespace AcceptanceTests.Configuration;

public class AsynchronousSideEffectsAwaiter(ScenarioContext context)
{
    public async Task WaitForSideEffects()
    {
        while (await context.GetService<FirstOrDefaultEventPublisher>().PublishFirstOrDefaultEvent(CancellationToken.None))
        { }
    }
}
