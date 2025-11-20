using SessionizeCfpViewer.Models;
using System.Text.Json;

namespace SessionizeCfpViewer.Services;

public class SessionizeApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SessionizeApiService> _logger;
    private const string ApiUrl = "https://sessionize.com/api/universal/open-cfps";

    public SessionizeApiService(HttpClient httpClient, ILogger<SessionizeApiService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        string _apiKey = configuration["API_TOKEN"];

        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("API_TOKEN is not configured. Please add API_TOKEN to your appsettings.json or user secrets.");
        }

        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
    }

    public async Task<List<CfpSession>> GetOpenCfpsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching open CFPs from Sessionize API");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var sessions = await _httpClient.GetFromJsonAsync<List<CfpSession>>(ApiUrl, options);

            if (sessions != null)
            {
                _logger.LogInformation("Successfully fetched {Count} CFP sessions", sessions.Count);
                return sessions;
            }
            _logger.LogWarning("No sessions returned from API");
            return new List<CfpSession>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching CFPs from Sessionize API");
            throw;
        }
    }
}
