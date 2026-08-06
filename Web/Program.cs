using Database.Migrations;
using Infrastructure;
using RMediator.DependencyInjection;
using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .Services
    .AddTicTacToeServices(new ConfigurationBuilder().AddEnvironmentVariables().Build())
    .AddSingleton<DomainEventComponentListeners>()
    .AddMediator(o => o.ScanAssemblies(typeof(Program).Assembly));

var app = builder.Build();
await app.Services.GetRequiredService<IMigrateDatabase>().Migrate();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
