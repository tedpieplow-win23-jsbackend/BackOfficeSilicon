using BackOfficeSilicon.Models;
using Newtonsoft.Json;
using System.Text;
using static BackOfficeSilicon.Components.BackOffice.UserForm;
using static System.Net.WebRequestMethods;

namespace BackOfficeSilicon.Services;

public class AppUsersService
{
    private readonly HttpClient Http;

    public AppUsersService(HttpClient http)
    {
        Http = http;
    }

    public async Task<IEnumerable<UserModel>> GetUsersAsync()
    {
        try
        {
            var result = await Http.PostAsync("https://userprovider-silicon.azurewebsites.net/api/GetUsersFunction?code=mO8A8H66WCIbodND5EnyTvnYdLJRR48RFtczOpczn60_AzFudJZ9Fw%3D%3D", null);
            if (result.IsSuccessStatusCode)
            {
                var list = await result.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();
                if (list!.Any())
                    return list!;
            }
        }
        catch (Exception ex)
        {
        }
        return null!;
    }

    public async Task<UserModel> GetOneUserAsync(string email)
    {
        try
        {
            var requestData = new { email = email };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await Http.PostAsync("https://userprovider-silicon.azurewebsites.net/api/GetUsersFunction?code=mO8A8H66WCIbodND5EnyTvnYdLJRR48RFtczOpczn60_AzFudJZ9Fw%3D%3D", content);

            if (result.IsSuccessStatusCode)
            {
                var user = await result.Content.ReadFromJsonAsync<UserModel>();
                return user!;
            }

        }
        catch (Exception ex)
        {
        }
        return null!;
    }
}
