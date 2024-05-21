using BackOfficeSilicon.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Claims;
namespace BackOfficeSilicon.Services;

public class UserService(HttpClient httpClient, IConfiguration configuration, UserManager<ApplicationUser> userManager, AuthenticationStateProvider stateProvider, IServiceScopeFactory scopeFactory, ApplicationDbContext context)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly AuthenticationStateProvider _stateProvider = stateProvider;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ApplicationDbContext _context = context;

    public async Task<ApplicationUser> GetUserAsync()
    {
        var scope = _scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var userclaims = await GetUserClaimsAsync();
        var user = await userManager.GetUserAsync(userclaims);

        if (user != null)
            return user;

        return null!;
    }

    public async Task<ClaimsPrincipal> GetUserClaimsAsync()
    {
        var authenticationState = await _stateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;

        if (user is not null)
        {
            return user;
        }
        return null!;
    }

    public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            _context.Entry(existingUser).State = EntityState.Detached;
        }

        _context.Users.Attach(user);
        _context.Entry(user).State = EntityState.Modified;

        return await _userManager.UpdateAsync(user);
    }
    public async Task<IdentityResult> UpdatePassUserAsync(ApplicationUser user, string oldPass, string newPass)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            _context.Entry(existingUser).State = EntityState.Detached;
        }

        _context.Users.Attach(user);
        _context.Entry(user).State = EntityState.Modified;

        return await _userManager.ChangePasswordAsync(user, oldPass, newPass);
    }
}
