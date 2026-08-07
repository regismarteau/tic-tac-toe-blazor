using Database.Migrations;
using Infrastructure;
using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .Services
    .AddTicTacToeServices(new ConfigurationBuilder().AddEnvironmentVariables().Build())
    .AddWebServices();

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
