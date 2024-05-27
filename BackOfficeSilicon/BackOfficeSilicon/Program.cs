using BackOfficeSilicon.Components;
using BackOfficeSilicon.Configurations;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder.Configuration);
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.MapHub<ChatHub>("/chathub");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BackOfficeSilicon.Client._Imports).Assembly);

//app.MapAdditionalIdentityEndpoints();

app.Run();
