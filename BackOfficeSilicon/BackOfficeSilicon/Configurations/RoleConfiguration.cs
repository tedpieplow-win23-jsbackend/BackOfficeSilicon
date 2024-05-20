using Microsoft.AspNetCore.Identity;

namespace BackOfficeSilicon.Configurations;

public static class RoleConfiguration
{
    public static async Task RegisterRoles(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = ["SuperUser", "Administrator", "Author", "User"];
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
        }
    }
}
