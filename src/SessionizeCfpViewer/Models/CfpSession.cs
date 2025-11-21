using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SessionizeCfpViewer.Models;

public class CfpSession
{
    [JsonPropertyName("eventId")]
    public int EventId { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("organizer")]
    public string? Organizer { get; set; }
    [JsonPropertyName("website")]
    public string? Website { get; set; }
    [JsonPropertyName("cfpLink")]
    public string? CfpLink { get; set; }

    public string? Description { get; set; }
    [JsonPropertyName("isTest")]
    public bool IsTest { get; set; }
    [JsonPropertyName("isOnline")]
    public bool IsOnline { get; set; }
    [JsonPropertyName("isUserGroup")]
    public bool IsUserGroup { get; set; }
    [JsonPropertyName("expensesCovered")]
    public ExpensesCovered? ExpensesCovered { get; set; }
    [JsonPropertyName("eventDates")]
    public EventDates? EventDates { get; set; }
    [JsonPropertyName("cfpDates")]
    public CfpDates? CfpDates { get; set; }
    [JsonPropertyName("timezone")]
    public TimeZoneEntity? Timezone { get; set; } // Changed type for assignment compatibility
    [JsonPropertyName("location")]
    public Location? Location { get; set; }
    [JsonPropertyName("links")]
    public Links? Links { get; set; }
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }
    [JsonPropertyName("city")]
    public string? City { get; set; }
    [JsonPropertyName("tags")]
    public string? Tags { get; set; }
    [JsonPropertyName("topics")]
    public string? Topics { get; set; }
    [JsonPropertyName("sessionFormats")]
    public string? SessionFormats { get; set; }
    [JsonPropertyName("categories")]
    public string? Categories { get; set; }
    [JsonPropertyName("isPaid")]
    public bool IsPaid { get; set; }
    [JsonPropertyName("lastUpdated")]
    public DateTime? LastUpdated { get; set; }
    [JsonPropertyName("timezoneJson")]
    public TimezoneInfo? TimezoneJson { get; set; }
    [JsonPropertyName("timeZoneId")]
    public int? TimeZoneId { get; set; }

    // Convenience properties for date handling
    public DateTime? CfpStartDate => CfpDates?.Start;
    public DateTime? CfpEndDate => CfpDates?.End;
    public DateTime? EventStartDate => DateTime.TryParse(EventDates?.Start, out var dt) ? dt : (DateTime?)null;
    public DateTime? EventEndDate => DateTime.TryParse(EventDates?.End, out var dt) ? dt : (DateTime?)null;

    // Computed property for filtering
    public bool IsOpen => CfpStartDate.HasValue && CfpEndDate.HasValue && DateTime.Now >= CfpStartDate.Value && DateTime.Now <= CfpEndDate.Value;

    // Computed properties for UI logic
    public bool IsInPerson => !IsOnline;
    public bool IsHybrid => IsOnline && Location != null && !string.IsNullOrEmpty(Location.Full);
    public bool IsFree => ExpensesCovered != null && ExpensesCovered.ConferenceFee;

    // Computed property to determine if conference is likely online-only
    // Assumes online-only if: 1) Location is null, OR 2) Event duration > 10 days
    public bool IsLikelyOnlineOnly
    {
        get
        {
            if (Location == null)
                return true;

            if (EventStartDate.HasValue && EventEndDate.HasValue)
            {
                var duration = (EventEndDate.Value - EventStartDate.Value).Days;
                if (duration > 10)
                    return true;
            }

            return false;
        }
    }

    // Convenience property for Razor and search usage
    public string? LocationString => Location?.Full;
}

public class TimeZoneEntity
{
    [Key]
    public int Id { get; set; } // Autoincrementing primary key
    public string? Iana { get; set; }
    public string? Windows { get; set; }
    public string? DisplayName { get; set; }
    public int UtcOffsetMinutes { get; set; }
}

public class TimezoneInfo
{
    [JsonPropertyName("iana")]
    public string? Iana { get; set; }
    [JsonPropertyName("windows")]
    public string? Windows { get; set; }
}

public class ExpensesCovered
{
    [JsonPropertyName("conferenceFee")]
    public bool ConferenceFee { get; set; }
    [JsonPropertyName("accommodation")]
    public bool Accommodation { get; set; }
    [JsonPropertyName("travel")]
    public bool Travel { get; set; }
}

public class EventDates
{
    [JsonPropertyName("start")]
    public string? Start { get; set; }
    [JsonPropertyName("end")]
    public string? End { get; set; }
    [JsonPropertyName("allDates")]
    public string[]? AllDates { get; set; }
}

public class CfpDates
{
    [JsonPropertyName("startUtc")]
    public DateTime StartUtc { get; set; }
    [JsonPropertyName("endUtc")]
    public DateTime EndUtc { get; set; }
    [JsonPropertyName("start")]
    public DateTime Start { get; set; }
    [JsonPropertyName("end")]
    public DateTime End { get; set; }
}

public class Timezone
{
    [JsonPropertyName("iana")]
    public string? Iana { get; set; }
    [JsonPropertyName("windows")]
    public string? Windows { get; set; }
}

public class Location
{
    [JsonPropertyName("full")]
    public string? Full { get; set; }
    [JsonPropertyName("city")]
    public string? City { get; set; }
    [JsonPropertyName("state")]
    public string? State { get; set; }
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    [JsonPropertyName("coordinates")]
    public string? Coordinates { get; set; }

    // Convenience ToString for search
    public override string? ToString() => Full;
}

public class Links
{
    [JsonPropertyName("twitter")]
    public string? Twitter { get; set; }
    [JsonPropertyName("linkedIn")]
    public string? LinkedIn { get; set; }
    [JsonPropertyName("facebook")]
    public string? Facebook { get; set; }
    [JsonPropertyName("instagram")]
    public string? Instagram { get; set; }
}
