using BackOfficeSilicon.Data;
using BackOfficeSilicon.Models;
using Microsoft.AspNetCore.Identity;

namespace BackOfficeSilicon.Services;

public class AdminService(UserManager<ApplicationUser> userManager, ILogger<AdminService> logger)
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AdminService> _logger = logger;

    public async Task<List<AdministratorModel>> PopulateAdminListAsync()
    {
        try
        {
            var adminModel = new AdministratorModel();
            var admins = _userManager.Users.ToList();
            var adminList = new List<AdministratorModel>();

            if (admins.Count != 0)
            {
                foreach (var admin in admins)
                {
                    var userRole = await _userManager.GetRolesAsync(admin);

                    adminModel.Email = admin.Email!;
                    adminModel.FirstName = admin.FirstName;
                    adminModel.LastName = admin.LastName;
                    adminModel.Role = userRole.FirstOrDefault()!;

                    adminList.Add(adminModel);
                }
            }

            return adminList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null!;
        }
    }
}
