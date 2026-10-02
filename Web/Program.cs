using Database.Migrations;
using Infrastructure;
using Web;
using Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .Services
    .AddTicTacToeServices(builder.Configuration)
    .AddWebServices();

var app = builder.Build();
await app.Services.GetRequiredService<IMigrateDatabase>().Migrate(app.Lifetime.ApplicationStopping);

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
