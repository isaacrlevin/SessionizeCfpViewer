using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SessionizeCfpViewer.Models;

namespace SessionizeCfpViewer.Services;

public class SessionizeApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SessionizeApiService> _logger;
    private const string ApiUrl = "https://sessionize.com/api/universal/open-cfps";
    private readonly string? _apiKey;

    public SessionizeApiService(HttpClient httpClient, ILogger<SessionizeApiService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = configuration["API_TOKEN"];
        
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("API_TOKEN is not configured. Please add API_TOKEN to your appsettings.json or user secrets.");
        }
        
        _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
    }
    
    public async Task<List<CfpSessionFlat>> GetOpenCfpsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching open CFPs from Sessionize API");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var json  = await _httpClient.GetStringAsync(ApiUrl);

            var sessions = await _httpClient.GetFromJsonAsync<List<CfpSession>>(ApiUrl, options);
            var flatList = new List<CfpSessionFlat>();

            if (sessions != null)
            {
                foreach (var session in sessions)
                {
                    var flat = new CfpSessionFlat
                    {
                        Id = session.Id,
                        EventId = session.EventId,
                        Name = session.Name,
                        Organizer = session.Organizer,
                        Website = session.Website,
                        CfpLink = session.CfpLink,
                        Description = session.Description,
                        IsTest = session.IsTest,
                        IsOnline = session.IsOnline,
                        IsUserGroup = session.IsUserGroup,
                        ExpensesConferenceFee = session.ExpensesCovered?.ConferenceFee ?? false,
                        ExpensesAccommodation = session.ExpensesCovered?.Accommodation ?? false,
                        ExpensesTravel = session.ExpensesCovered?.Travel ?? false,
                        EventStartDate = DateTime.TryParse(session.EventDates?.Start, out var estart) ? estart : (DateTime?)null,
                        EventEndDate = DateTime.TryParse(session.EventDates?.End, out var eend) ? eend : (DateTime?)null,
                        EventAllDates = session.EventDates?.AllDates != null ? string.Join(",", session.EventDates.AllDates) : null,
                        CfpStartDate = session.CfpDates?.Start,
                        CfpEndDate = session.CfpDates?.End,
                        CfpStartUtc = session.CfpDates?.StartUtc,
                        CfpEndUtc = session.CfpDates?.EndUtc,
                        TimezoneIana = session.Timezone?.Iana,
                        TimezoneWindows = session.Timezone?.Windows,
                        TimeZoneId = session.TimeZoneId,
                        LocationFull = session.Location?.Full,
                        LocationCity = session.Location?.City,
                        LocationState = session.Location?.State,
                        LocationCountry = session.Location?.Country,
                        LocationCoordinates = session.Location?.Coordinates,
                        Country = session.Country,
                        CountryCode = session.CountryCode,
                        City = session.City,
                        Tags = session.Tags,
                        Topics = session.Topics,
                        SessionFormats = session.SessionFormats,
                        Categories = session.Categories,
                        IsPaid = session.IsPaid,
                        LastUpdated = DateTime.UtcNow,
                        LinksTwitter = session.Links?.Twitter,
                        LinksLinkedIn = session.Links?.LinkedIn,
                        LinksFacebook = session.Links?.Facebook,
                        LinksInstagram = session.Links?.Instagram
                    };
                    flatList.Add(flat);
                }
                _logger.LogInformation("Successfully fetched {Count} CFP sessions", flatList.Count);
                return flatList;
            }
            _logger.LogWarning("No sessions returned from API");
            return new List<CfpSessionFlat>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching CFPs from Sessionize API");
            throw;
        }
    }
}
