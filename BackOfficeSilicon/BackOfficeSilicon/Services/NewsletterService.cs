using BackOfficeSilicon.Models;
using Newtonsoft.Json;
using System.Text;

using static System.Net.WebRequestMethods;

namespace BackOfficeSilicon.Services;

public class NewsletterService(HttpClient http)
{
    private readonly HttpClient Http = http;

    public async Task<IEnumerable<NewsletterModel>> GetSubscribersAsync()
    {
        try
        {
            var result = await Http.PostAsync("https://subscriptionprovider-silicon.azurewebsites.net/api/GetSubscribersFunction?code=VdUem1ardDpuXjfbHNodWR6NRuTneq6ZTFo3n_8r7fHZAzFutbdzXA==", null);
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
            var result = await Http.PostAsync("https://subscriptionprovider-silicon.azurewebsites.net/api/GetSubscribersFunction?code=VdUem1ardDpuXjfbHNodWR6NRuTneq6ZTFo3n_8r7fHZAzFutbdzXA==", content);
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
        var result = await Http.PostAsync("https://subscriptionprovider-silicon.azurewebsites.net/api/UpdateSubscriberFunction?code=bdOaYycwsJU9tdvHb_O_8pnk56JT8VFcZ7QpqqTrp5pRAzFuV3OoQQ%3D%3D", content);
        if (result.IsSuccessStatusCode)
            return true;
        return false;
    }
}
