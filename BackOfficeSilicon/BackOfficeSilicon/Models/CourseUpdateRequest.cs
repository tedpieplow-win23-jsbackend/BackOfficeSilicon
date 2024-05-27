namespace BackOfficeSilicon.Models;

public class CourseUpdateRequest
{
    public string Id { get; set; } = null!;
    public string? ImageUri { get; set; }
    public string? ImageHeaderUri { get; set; }
    public bool IsBestseller { get; set; }
    public bool IsDigital { get; set; }
    public List<string>? Categories { get; set; }
    public string? Title { get; set; }
    public string? Ingress { get; set; }
    public decimal StarRating { get; set; }
    public string? Reviews { get; set; }
    public string? LikesInPercent { get; set; }
    public string? Likes { get; set; }
    public string? Hours { get; set; }
    // We use virtual due to LazyLoading
    public List<AuthorUpdateRequest>? Authors { get; set; }
    public PricesUpdateRequest? Prices { get; set; }
    public ContentUpdateRequest? Content { get; set; }
}

public class AuthorUpdateRequest
{
    public string? Name { get; set; }
}

public class ContentUpdateRequest
{
    public string? Description { get; set; }
    public List<string>? Includes { get; set; }
    public List<ProgramDetailItemUpdateRequest>? ProgramDetails { get; set; }
}

public class PricesUpdateRequest
{
    public string? Currency { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
}

public class ProgramDetailItemUpdateRequest
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}
