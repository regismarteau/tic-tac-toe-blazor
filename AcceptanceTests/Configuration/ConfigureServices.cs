using AcceptanceTests.ErrorHandling;
using Database;
using Database.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Reqnroll;
using RMediator.DependencyInjection;

namespace AcceptanceTests.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection SubstituteServices(this IServiceCollection services, ScenarioContext context) => services
        .AddScoped<AsynchronousSideEffectsAwaiter>()
        .AddSingleton(context)
        .SubstituteDatabase()
        .AddMediator(o => o.AddMiddlewares(typeof(AcceptanceErrorHandling<>), typeof(AcceptanceErrorHandling<,>)));

    private static IServiceCollection SubstituteDatabase(this IServiceCollection services)
    {
        var databaseName = Guid.NewGuid().ToString();
        return services.AddScoped(_ => new DbContextOptionsBuilder<TicTacToeDbContext>()
                .UseInMemoryDatabase(databaseName).Options)
            .AddSingleton(_ =>
            {
                var substitute = Substitute.For<IMigrateDatabase>();
                substitute.Migrate().ReturnsForAnyArgs(Task.CompletedTask);
                return substitute;
            });
    }
}
