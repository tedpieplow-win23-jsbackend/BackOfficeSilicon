using BackOfficeSilicon.Models;
using Newtonsoft.Json;
using System.Text;


namespace BackOfficeSilicon.Services;

public class NewsletterService(HttpClient http, IConfiguration configuration)
{
    private readonly HttpClient Http = http;
    private readonly IConfiguration _configuration = configuration;
    public async Task<IEnumerable<NewsletterModel>> GetSubscribersAsync()
    {
        try
        {
            var result = await Http.PostAsync(_configuration["GetNewsletterProvider"], null);
            if (result.IsSuccessStatusCode)
            {
                var list = await result.Content.ReadFromJsonAsync<IEnumerable<NewsletterModel>>();
                if (list!.Any())
                    return list!;
            }
        }
        catch (Exception ex)
        {
        }
        return null!;
    }
    public async Task<NewsletterModel> GetSubscribersAsync(string email)
    {
        try
        {
            var requestData = new { Email = email };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await Http.PostAsync(_configuration["GetNewsletterProvider"], content);
            if (result.IsSuccessStatusCode)
            {
                var model = await result.Content.ReadFromJsonAsync<NewsletterModel>();
                return model!;
            }
        }
        catch (Exception ex)
        {
        }
        return null!;
    }
    public async Task<bool> UpdateSubscriberAsync(NewsletterModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await Http.PostAsync(_configuration["UpdateNewsletterProvider"], content);
        if (result.IsSuccessStatusCode)
            return true;
        return false;
    }
    public async Task<IEnumerable<NewsletterModel>> DeleteAsync(string email)
    {
        var json = JsonConvert.SerializeObject(new { Email = email });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await Http.PostAsync(_configuration["DeleteNewsletterProvider"], content);

        var result = await GetSubscribersAsync();
        if(result.Any()) 
            return result;
        return null!;
    }
}
