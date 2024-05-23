using BackOfficeSilicon.Components.Account;
using BackOfficeSilicon.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Blazored.Modal;
using BackOfficeSilicon.Services;
using Silicon.Blazor.Services;

namespace BackOfficeSilicon.Configurations;

public static class ServiceConfiguration
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorComponents()
              .AddInteractiveServerComponents()
              .AddInteractiveWebAssemblyComponents();

        services.AddHttpClient();
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
        services.AddScoped<ClaimsPrincipal>();
        services.AddScoped<NewsletterService>();
        services.AddScoped<UserService>();
        services.AddScoped<CookieEvents>();
        services.AddScoped<AdminService>();
        services.AddScoped<CourseService>();
        services.AddScoped<AppUsersService>();

        services.AddBlazoredModal();

        services.AddHttpContextAccessor();

        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
            .AddIdentityCookies();

        services.ConfigureApplicationCookie(options =>
        {
            options.EventsType = typeof(CookieEvents);
        });

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddRoleStore<RoleStore<IdentityRole, ApplicationDbContext>>()
            .AddDefaultTokenProviders();
    }
}
