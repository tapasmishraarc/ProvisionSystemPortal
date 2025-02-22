using System.Text;
using System.Text.Json;
using SystemProvisioningPortal.Models;

namespace SystemProvisioningPortal.Services;

public class AzureDevOpsService : IAzureDevOpsService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AzureDevOpsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        var pat = _configuration["AzureDevOps:PersonalAccessToken"];
        var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}"));
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);
        _httpClient.BaseAddress = new Uri(_configuration["AzureDevOps:ApiUrl"] ?? throw new InvalidOperationException("Azure DevOps API URL not configured"));
    }

    public async Task<object> TriggerProvisioningPipelineAsync(ProvisioningRequest request)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/pipelines/provision/runs", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<object>() ?? 
                throw new InvalidOperationException("Failed to deserialize response");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Failed to trigger provisioning pipeline: {ex.Message}");
        }
    }
}