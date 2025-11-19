using System;
using System.ComponentModel.DataAnnotations;

namespace SessionizeCfpViewer.Models;

public class CfpSessionFlat
{
    [Key]
    public int Id { get; set; }
    public int EventId { get; set; }
    public string? Name { get; set; }
    public string? Organizer { get; set; }
    public string? Website { get; set; }
    public string? CfpLink { get; set; }

    public string? Description { get; set; }
    public bool IsTest { get; set; }
    public bool IsOnline { get; set; }
    public bool IsUserGroup { get; set; }
    public bool ExpensesConferenceFee { get; set; }
    public bool ExpensesAccommodation { get; set; }
    public bool ExpensesTravel { get; set; }
    public DateTime? EventStartDate { get; set; }
    public DateTime? EventEndDate { get; set; }
    public string? EventAllDates { get; set; } // Comma-separated
    public DateTime? CfpStartDate { get; set; }
    public DateTime? CfpEndDate { get; set; }
    public DateTime? CfpStartUtc { get; set; }
    public DateTime? CfpEndUtc { get; set; }
    public string? TimezoneIana { get; set; }
    public string? TimezoneWindows { get; set; }
    public int? TimeZoneId { get; set; }
    public string? LocationFull { get; set; }
    public string? LocationCity { get; set; }
    public string? LocationState { get; set; }
    public string? LocationCountry { get; set; }
    public string? LocationCoordinates { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string? City { get; set; }
    public string? Tags { get; set; }
    public string? Topics { get; set; }
    public string? SessionFormats { get; set; }
    public string? Categories { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? LinksTwitter { get; set; }
    public string? LinksLinkedIn { get; set; }
    public string? LinksFacebook { get; set; }
    public string? LinksInstagram { get; set; }
}
