namespace BackOfficeSilicon.Configurations;

public static class AuthorizationConfiguration
{
    public static void RegisterAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(x =>
        {
            x.AddPolicy("SuperUser", policy => policy.RequireRole("SuperUser"));
            x.AddPolicy("Administrator", policy => policy.RequireRole("SuperUser", "Administrator"));
            x.AddPolicy("Author", policy => policy.RequireRole("SuperUser", "Administrator", "Author"));
            x.AddPolicy("User", policy => policy.RequireRole("SuperUser", "Administrator", "Author", "User"));
        });
    }
}
