using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackOfficeSilicon.Data;

public class ApplicationUser : IdentityUser
{
    [Required]
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [Required]
    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;
}
