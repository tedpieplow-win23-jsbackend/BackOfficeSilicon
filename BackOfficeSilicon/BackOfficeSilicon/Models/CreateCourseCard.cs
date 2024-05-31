using System.ComponentModel.DataAnnotations;

namespace BackOfficeSilicon.Models;

public class CreateCourseCard
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required(ErrorMessage = "ImageUri is required")]
    public string? ImageUri { get; set; }
    [Required(ErrorMessage = "ImageHeaderUri is required")]
    public string? ImageHeaderUri { get; set; }
    public bool IsBestseller { get; set; }
    public bool IsDigital { get; set; }
    public string[]? Categories { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "Ingress is required")]
    public string? Ingress { get; set; }
    [Required(ErrorMessage = "StarRating is required")]
    public decimal StarRating { get; set; }
    [Required(ErrorMessage = "Reviews is required")]
    public string? Reviews { get; set; }
    [Required(ErrorMessage = "LikesInPercent is required")]
    public string? LikesInPercent { get; set; }
    [Required(ErrorMessage = "Likes is required")]
    public string? Likes { get; set; }
    [Required(ErrorMessage = "Hours is required")]
    public string? Hours { get; set; }
    [Required(ErrorMessage = "Author name is required")]
    public virtual List<AuthorCreateRequest>? Authors { get; set; }
    public virtual PricesCreateRequest? Prices { get; set; }
    [Required(ErrorMessage = "Content is required")]
    public virtual ContentCreateRequest? Content { get; set; }
}

public class AuthorCreateRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
}
public class ContentCreateRequest
{
    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "Includes is required")]
    public string[]? Includes { get; set; }
    public virtual List<ProgramDetailItemCreateRequest>? ProgramDetails { get; set; }
}

public class PricesCreateRequest
{
    public string? Currency { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
}

public class ProgramDetailItemCreateRequest
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }
}
