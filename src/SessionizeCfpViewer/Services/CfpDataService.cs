using SessionizeCfpViewer.Models;

namespace SessionizeCfpViewer.Services;

public class CfpDataService
{
    private readonly SessionizeApiService _apiService;
    private readonly ILogger<CfpDataService> _logger;
    private List<CfpSession> _sessions = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public CfpDataService(
        SessionizeApiService apiService,
        ILogger<CfpDataService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task RefreshDataAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            _logger.LogInformation("Starting data refresh from Sessionize API");

            var sessions = await _apiService.GetOpenCfpsAsync();

            // Filter out sessions with null or empty Website
            var validSessions = sessions.Where(s => !string.IsNullOrWhiteSpace(s.Website)).ToList();

            // Replace in-memory data
            _sessions = validSessions;

            _logger.LogInformation("Successfully refreshed {Count} CFP sessions", validSessions.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing CFP data");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public Task<List<CfpSession>> GetAllSessionsAsync()
    {
        return Task.FromResult(_sessions.ToList());
    }

    public Task<CfpSession?> GetSessionByIdAsync(string id)
    {
        if (int.TryParse(id, out var intId))
        {
            var session = _sessions.FirstOrDefault(s => s.EventId == intId);
            return Task.FromResult(session);
        }
        return Task.FromResult<CfpSession?>(null);
    }

    public Task<List<CfpSession>> SearchSessionsAsync(
        string? searchTerm = null,
        bool openOnly = true,
        string? sortBy = "cfpEndDate",
        bool ascending = true)
    {
        var query = _sessions.AsQueryable();

        // Filter by open status
        if (openOnly)
        {
            var now = DateTime.Now;
            query = query.Where(s =>
                s.CfpStartDate.HasValue &&
                s.CfpEndDate.HasValue &&
                s.CfpStartDate.Value <= now &&
                s.CfpEndDate.Value >= now);
        }

        // Search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(s =>
                (s.Name != null && s.Name.ToLower().Contains(searchTerm)) ||
                (s.Location != null && !string.IsNullOrEmpty(s.Location.Full) && s.Location.Full.ToLower().Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(s.Country) && s.Country.ToLower().Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(s.City) && s.City.ToLower().Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(s.Topics) && s.Topics.ToLower().Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(s.Tags) && s.Tags.ToLower().Contains(searchTerm)));
        }

        // Sorting
        if (sortBy?.ToLower() == "country")
        {
            // Materialize query first for country sort to handle nulls
            var materializedQuery = query.ToList();
            query = ascending 
                ? materializedQuery.OrderBy(s => s.Location?.Country).AsQueryable()
                : materializedQuery.OrderByDescending(s => s.Location?.Country).AsQueryable();
        }
        else
        {
            query = sortBy?.ToLower() switch
            {
                "name" => ascending ? query.OrderBy(s => s.Name) : query.OrderByDescending(s => s.Name),
                "cfpenddate" => ascending ? query.OrderBy(s => s.CfpEndDate) : query.OrderByDescending(s => s.CfpEndDate),
                "cfpstartdate" => ascending ? query.OrderBy(s => s.CfpStartDate) : query.OrderByDescending(s => s.CfpStartDate),
                "eventstartdate" => ascending ? query.OrderBy(s => s.EventStartDate) : query.OrderByDescending(s => s.EventStartDate),
                _ => ascending ? query.OrderBy(s => s.CfpEndDate) : query.OrderByDescending(s => s.CfpEndDate)
            };
        }

        return Task.FromResult(query.ToList());
    }
}
