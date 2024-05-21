using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BackOfficeSilicon.Data;

public class ApplicationUser : IdentityUser
{
    [Required]
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [Required]
    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;

    [ProtectedPersonalData]
    public string? Biography { get; set; }
    public string? ProfileImageUrl { get; set; }
}
