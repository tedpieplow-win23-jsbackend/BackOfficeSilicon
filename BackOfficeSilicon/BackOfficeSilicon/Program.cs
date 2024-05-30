using BackOfficeSilicon.Components;
using BackOfficeSilicon.Configurations;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

//// Configure the client
//var client = new SecretClient(new Uri("https://backofficesiliconkv.vault.azure.net/"), new DefaultAzureCredential());

//// Retrieve a secret
//KeyVaultSecret secret = client.GetSecret("VaultUri");
//Console.WriteLine(secret.Value);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

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
