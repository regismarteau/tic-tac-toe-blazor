using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;

namespace AcceptanceTests.Configuration;

public class TestServices(ScenarioContext context) : IDisposable
{
    private readonly IServiceScope scope = new ServiceCollection()
            .AddTicTacToeServices(new ConfigurationBuilder().Build())
            .SubstituteServices(context)
            .BuildServiceProvider(true)
            .CreateScope();

    private bool disposedValue;

    public T GetService<T>() where T : notnull
    {
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                scope.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
