using System.ComponentModel.DataAnnotations;

namespace BackOfficeSilicon.Models;

public class AdministratorModel
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid e-mail address.")]
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set;} = null!;
    public string Role { get; set; } = null!;
}
