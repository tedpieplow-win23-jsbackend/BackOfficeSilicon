namespace BackOfficeSilicon.Models;

public class AdministratorModel
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set;} = null!;
    public IEnumerable<string> Roles { get; set; } = [];
    public string PhoneNumber { get; set; } = null!;
    public string Biography { get; set; } = null!;
    public string ProfileImageUrl { get; set; } = null!;
}
