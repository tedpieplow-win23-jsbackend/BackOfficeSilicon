using BackOfficeSilicon.Models;
using static System.Net.WebRequestMethods;
using System.Text.Json;

namespace BackOfficeSilicon.Services;

public class CourseService(HttpClient http, IConfiguration configuration)
{
    private readonly HttpClient _http = http;
    private readonly IConfiguration _configuration = configuration;

    public async Task<CourseCard> GetCourseToUpdateByIdAsync(string courseId)
    {
        var query = new GraphQLQuery
        {
            Query = @"
            query getCourseById($id: String!) {
                getCourseById(id: $id) {
                    id
                    imageUri
                    imageHeaderUri
                    isBestseller
                    isDigital
                    categories
                    title
                    ingress
                    starRating
                    reviews
                    likesInPercent
                    likes
                    hours
                    authors {
                      name
                    }
                    content {
                      description
                      includes
                      programDetails {
                        id
                        title
                        description
                      }
                    }
                    prices {
                      currency
                      price
                      discount
                    }
                  }
            }",
            Variables = new { id = $"{courseId}" }
        };

        var response = await _http.PostAsJsonAsync(_configuration.GetValue<string>("ConnectionStrings:GetCoursesProvider"), query);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<DynamicGraphQLResponse>();

            if (result != null && result.Data.TryGetProperty("getCourseById", out var courseData))
            {
                return new CourseCard
                {
                    Id = courseData.GetProperty("id").GetString() ?? "",
                    Hours = courseData.GetProperty("hours").GetString() ?? "",
                    ImageHeaderUri = courseData.GetProperty("imageHeaderUri").GetString() ?? "",
                    ImageUri = courseData.GetProperty("imageUri").GetString() ?? "",
                    Ingress = courseData.GetProperty("ingress").GetString() ?? "",
                    IsBestseller = courseData.GetProperty("isBestseller").GetBoolean(),
                    IsDigital = courseData.GetProperty("isDigital").GetBoolean(),
                    Likes = courseData.GetProperty("likes").GetString() ?? "",
                    LikesInPercent = courseData.GetProperty("likesInPercent").GetString() ?? "",
                    Reviews = courseData.GetProperty("reviews").GetString() ?? "",
                    StarRating = courseData.GetProperty("starRating").GetDecimal(),
                    Title = courseData.GetProperty("title").GetString() ?? "",
                    Authors = courseData.TryGetProperty("authors", out var authors) && authors.GetArrayLength() > 0
                                    ? authors.EnumerateArray().Select(a => new Author { Name = a.GetProperty("name").GetString() ?? "" }).ToList()
                                    : new List<Author>(),
                    Content = new Content
                    {
                        Description = courseData.GetProperty("content").GetProperty("description").GetString() ?? "",
                        Includes = courseData.TryGetProperty("content", out var content) && content.GetProperty("includes").GetArrayLength() > 0
                                            ? content.GetProperty("includes").EnumerateArray().Select(i => i.GetString()).ToList()!
                                            : new List<string>(),
                        ProgramDetails = courseData.TryGetProperty("content", out var contentDetails)
                                                && content.TryGetProperty("programDetails", out var programDetails)
                                                && programDetails.GetArrayLength() > 0
                                                    ? programDetails.EnumerateArray().Select(pd =>
                                                        new ProgramDetail
                                                        {
                                                            Id = pd.GetProperty("id").GetInt32(),
                                                            Description = pd.GetProperty("description").GetString() ?? "",
                                                            Title = pd.GetProperty("title").GetString() ?? ""
                                                        }).ToList()
                                                    : new List<ProgramDetail>()
                    },
                    Prices = new Prices
                    {
                        Currency = courseData.GetProperty("prices").GetProperty("currency").GetString() ?? "",
                        Discount = courseData.GetProperty("prices").GetProperty("discount").GetDecimal(),
                        Price = courseData.GetProperty("prices").GetProperty("price").GetDecimal()
                    }
                };
            }
        }
        return null!;
    }

    public async Task<CourseCard> UpdateCourseAsync(CourseCard input)
    {
        var query = new GraphQLQuery
        {
            Query = @"
            mutation UpdateCourse($input: CourseUpdateRequestInput!) {
                updateCourse(input: $input) {
                    id
                    imageUri
                    imageHeaderUri
                    isBestseller
                    isDigital
                    categories
                    title
                    ingress
                    starRating
                    reviews
                    likesInPercent
                    likes
                    hours
                    authors {
                        name
                    }
                    content {
                        description
                        includes
                        programDetails {
                            id
                            title
                            description
                        }
                    }
                    prices {
                        currency
                        price
                        discount
                    }
                }
            }",
            Variables = new { input }
        };

        try
        {
            var response = await _http.PostAsJsonAsync(_configuration.GetValue<string>("ConnectionStrings:GetCoursesProvider"), query);
            //var response = await _http.PostAsJsonAsync("http://localhost:7228/api/graphql", query);


            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var document = JsonDocument.Parse(jsonResponse);
                    var root = document.RootElement;

                    if (root.TryGetProperty("data", out var data) && data.TryGetProperty("updateCourse", out var courseData))
                    {
                        var updatedCourse = new CourseCard
                        {
                            Id = courseData.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
                            Hours = courseData.TryGetProperty("hours", out var hours) ? hours.GetString() ?? "" : "",
                            ImageHeaderUri = courseData.TryGetProperty("imageHeaderUri", out var imageHeaderUri) ? imageHeaderUri.GetString() ?? "" : "",
                            ImageUri = courseData.TryGetProperty("imageUri", out var imageUri) ? imageUri.GetString() ?? "" : "",
                            Ingress = courseData.TryGetProperty("ingress", out var ingress) ? ingress.GetString() ?? "" : "",
                            IsBestseller = courseData.TryGetProperty("isBestseller", out var isBestseller) && isBestseller.GetBoolean(),
                            IsDigital = courseData.TryGetProperty("isDigital", out var isDigital) && isDigital.GetBoolean(),
                            Likes = courseData.TryGetProperty("likes", out var likes) ? likes.GetString() ?? "" : "",
                            LikesInPercent = courseData.TryGetProperty("likesInPercent", out var likesInPercent) ? likesInPercent.GetString() ?? "" : "",
                            Reviews = courseData.TryGetProperty("reviews", out var reviews) ? reviews.GetString() ?? "" : "",
                            StarRating = courseData.TryGetProperty("starRating", out var starRating) ? starRating.GetDecimal() : 0,
                            Title = courseData.TryGetProperty("title", out var title) ? title.GetString() ?? "" : "",
                            Authors = courseData.TryGetProperty("authors", out var authors) && authors.ValueKind == JsonValueKind.Array
                                ? authors.EnumerateArray().Select(a => new Author { Name = a.TryGetProperty("name", out var name) ? name.GetString() ?? "" : "" }).ToList()
                                : new List<Author>(),
                            Content = courseData.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.Object
                                        ? new Content
                                        {
                                            Description = content.TryGetProperty("description", out var description) ? description.GetString() ?? "" : "",
                                            Includes = content.TryGetProperty("includes", out var includes) && includes.ValueKind == JsonValueKind.Array
                                                ? includes.EnumerateArray().Select(i => i.GetString()).ToList()!
                                                : new List<string>(),
                                            ProgramDetails = content.TryGetProperty("programDetails", out var programDetails) && programDetails.ValueKind == JsonValueKind.Array
                                                ? programDetails.EnumerateArray().Select(pd => new ProgramDetail
                                                {
                                                    Id = pd.TryGetProperty("id", out var pdId) ? pdId.GetInt32() : 0,
                                                    Title = pd.TryGetProperty("title", out var pdTitle) ? pdTitle.GetString() ?? "" : "",
                                                    Description = pd.TryGetProperty("description", out var pdDescription) ? pdDescription.GetString() ?? "" : ""
                                                }).ToList()
                                                : new List<ProgramDetail>()
                                        }
                                        : null,
                            Prices = courseData.TryGetProperty("prices", out var prices) && prices.ValueKind == JsonValueKind.Object
                                        ? new Prices
                                        {
                                            Currency = prices.TryGetProperty("currency", out var currency) ? currency.GetString() ?? "" : "",
                                            Discount = prices.TryGetProperty("discount", out var discount) ? discount.GetDecimal() : 0,
                                            Price = prices.TryGetProperty("price", out var price) ? price.GetDecimal() : 0
                                        }
                                        : null
                        };

                        return updatedCourse;
                    }
                    else
                    {
                        throw new InvalidOperationException("The result data does not contain the expected 'updateCourse' property.");
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateCourseAsync: {ex.Message}");
        }
        return null!;
    }

    public CourseUpdateRequest MapCourseCardToUpdateRequest(CourseCard courseInput, List<string> tempIncludes)
    {
        var input = new CourseUpdateRequest
        {
            Id = courseInput.Id,
            ImageUri = courseInput.ImageUri,
            ImageHeaderUri = courseInput.ImageHeaderUri,
            IsBestseller = courseInput.IsBestseller,
            IsDigital = courseInput.IsDigital,
            Categories = courseInput.Categories,
            Title = courseInput.Title,
            Ingress = courseInput.Ingress,
            StarRating = courseInput.StarRating,
            Reviews = courseInput.Reviews,
            LikesInPercent = courseInput.LikesInPercent,
            Likes = courseInput.Likes,
            Hours = courseInput.Hours,
            Authors = courseInput.Authors!.Select(a => new AuthorUpdateRequest { Name = a.Name }).ToList(),
            Content = new ContentUpdateRequest
            {
                Description = courseInput.Content!.Description,
                Includes = tempIncludes.ToList(),
                ProgramDetails = courseInput.Content.ProgramDetails!.Select(pd => new ProgramDetailItemUpdateRequest
                {
                    Id = pd.Id,
                    Title = pd.Title,
                    Description = pd.Description
                }).ToList()
            },
            Prices = new PricesUpdateRequest
            {
                Currency = courseInput.Prices!.Currency,
                Price = courseInput.Prices.Price,
                Discount = courseInput.Prices.Discount
            }
        };
        return input;
    }

    public async Task<CreateCourseCard> CreateNewCourseAsync(CreateCourseCard input)
    {
        var query = new GraphQLQuery
        {
            Query = @"
            mutation CreateCourse($input: CourseCreateRequestInput!) {
                createCourse(input: $input) {
                    id
                    imageUri
                    imageHeaderUri
                    isBestseller
                    isDigital
                    categories
                    title
                    ingress
                    starRating
                    reviews
                    likesInPercent
                    likes
                    hours
                    authors {
                        name
                    }
                    content {
                        description
                        includes
                        programDetails {
                            id
                            title
                            description
                        }
                    }
                    prices {
                        currency
                        price
                        discount
                    }
                }
            }",
            Variables = new { input }
        };

        try
        {
            var response = await _http.PostAsJsonAsync(_configuration.GetValue<string>("ConnectionStrings:GetCoursesProvider"), query);
            //var response = await _http.PostAsJsonAsync("http://localhost:7228/api/graphql", query);

            // Fixa visuella meddelanden. 
            if (response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    return input;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateCourseAsync: {ex.Message}");
        }
        return null!;
    }
}
