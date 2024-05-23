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
            var admins = _userManager.Users.ToList();
            var adminList = new List<AdministratorModel>();

            if (admins.Count != 0)
            {
                foreach (var admin in admins)
                {
                    var adminModel = new AdministratorModel();
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

    public async Task<IdentityResult> UpdateAdminAsync(ApplicationUser admin, AdminUpdateModel model)
    {
        try
        {
            admin.Email = model.Email;
            admin.FirstName = model.FirstName;
            admin.LastName = model.LastName;

            return await _userManager.UpdateAsync(admin);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return IdentityResult.Failed();
        }
    }

    public ApplicationUser PopulateAdmin(AdministratorModel model) 
    {
        return new ApplicationUser
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.Email,
        };
    }
}
