using SessionizeCfpViewer.Models;

namespace SessionizeCfpViewer.Services;

public class AppState
{
    private readonly ILogger<AppState> _logger;
    private DateTime? _lastRefreshUtc;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(15);
    private List<CfpSessionFlat> _sessions = new();

    public AppState(ILogger<AppState> logger)
    {
        _logger = logger;
    }

    public IReadOnlyList<CfpSessionFlat> Sessions => _sessions;
    public DateTime? LastRefreshUtc => _lastRefreshUtc;
    public bool IsInitialized => _lastRefreshUtc.HasValue;

    public async Task EnsureInitializedAsync(CfpDataService dataService)
    {
        if (!IsInitialized || DateTime.UtcNow - _lastRefreshUtc! > _cacheDuration)
        {
            _logger.LogInformation("AppState cache miss or expired (initialized={Initialized}). Refreshing from API.", IsInitialized);
            await RefreshInternalAsync(dataService);
        }
    }

    public async Task ForceRefreshAsync(CfpDataService dataService)
    {
        _logger.LogInformation("AppState forced refresh requested.");
        await RefreshInternalAsync(dataService);
    }

    private async Task RefreshInternalAsync(CfpDataService dataService)
    {
        await dataService.RefreshDataAsync();
        _sessions = await dataService.GetAllSessionsAsync();
        _lastRefreshUtc = DateTime.UtcNow;
        _logger.LogInformation("AppState refreshed. Cached {Count} sessions at {Timestamp} UTC.", _sessions.Count, _lastRefreshUtc);
    }
}
