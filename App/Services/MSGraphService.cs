using System.Net.Http.Headers;
using System.Text;
using App.Interfaces;
using App.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Services;

public class MSGraphService : IMSGraphService
{
    private readonly IConfiguration _configuration;
    private readonly IConfidentialClientApplication _client;
    private readonly HttpClient _httpClient;

    public MSGraphService(IConfiguration configuration)
    {
        _configuration = configuration;

        _client = ConfidentialClientApplicationBuilder.Create(_configuration.GetValue<string>("MSGraph:Account:ClientId"))
            .WithAuthority(string.Format(MSGraphUtilities.AuthorityFormat, _configuration.GetValue<string>("MSGraph:TenantId")))
            .WithClientSecret(_configuration.GetValue<string>("MSGraph:Account:ClientSecret"))
            .Build();

        _httpClient = new HttpClient();
    }

    public async Task<(bool Success, string Token, string Message)> GetTokenAsync()
    {
        try
        {
            var authResult = await _client.AcquireTokenForClient(new[] { MSGraphUtilities.MSGraphScope }).ExecuteAsync();

            return (true, authResult.AccessToken, null);
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }

    public async Task<(bool Success, Event Result, string Message)> CreateEventAsync(string token, Event item)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, string.Format(MSGraphUtilities.MSGraphQueryByUser, _configuration.GetValue<string>("MSGraph:UserId")));

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var serializedObject = JsonConvert.SerializeObject(item, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
            });

            request.Content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode) return (false, null, response.ReasonPhrase);

            var content = await response.Content.ReadAsStringAsync();

            return (true, JsonConvert.DeserializeObject<Event>(content), null);
        }
        catch (Exception ex)
        {
            return (false, null, ex.Message);
        }
    }
}