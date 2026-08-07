using RMediator.DependencyInjection;
using Web.Dispatchers;

namespace Web.Configurations;

public static class ConfigureServices
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddWebServices()
        {
            return services
                .AddSingleton<DomainEventComponentListeners>()
                .AddMediator(o => o.ScanAssemblies(typeof(Program).Assembly));
        }
    }
}