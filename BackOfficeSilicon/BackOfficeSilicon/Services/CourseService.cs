using BackOfficeSilicon.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static System.Net.WebRequestMethods;

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

    public async Task<CourseCard> UpdateCourseAsync()
    {
        return new CourseCard();
    }
}
