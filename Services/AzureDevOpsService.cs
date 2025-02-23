using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SystemProvisioningPortal.Models;

namespace SystemProvisioningPortal.Services;

public class AzureDevOpsService : IAzureDevOpsService
{
    private readonly HttpClient _httpClient;
    private readonly AzureDevOpsSettings _settings;

    public AzureDevOpsService(
        HttpClient httpClient,
        IOptions<AzureDevOpsSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;

        var auth = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($":{_settings.PersonalAccessToken}"));
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);
        _httpClient.BaseAddress = new Uri(_settings.ApiUrl);
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
