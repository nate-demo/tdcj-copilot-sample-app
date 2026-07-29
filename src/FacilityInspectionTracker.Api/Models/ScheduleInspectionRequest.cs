namespace FacilityInspectionTracker.Api.Models;

/// <summary>
/// Represents the payload for the <c>POST /api/inspections</c> endpoint
/// used to schedule a new facility inspection.
/// </summary>
public class ScheduleInspectionRequest
{
    /// <summary>Gets or sets the ID of the facility to be inspected.</summary>
    public int FacilityId { get; set; }

    /// <summary>Gets or sets the full name of the inspector assigned to this inspection.</summary>
    public string InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category of the inspection (e.g., "Fire Safety", "Structural Integrity").
    /// </summary>
    public string InspectionType { get; set; } = string.Empty;

    /// <summary>Gets or sets the date and time the inspection is scheduled to occur.</summary>
    public DateTime ScheduledDate { get; set; }

    /// <summary>
    /// Gets or sets the priority level as a case-insensitive string
    /// (e.g., "Low", "Medium", "High", "Critical"). Defaults to <c>"Medium"</c>
    /// if an unrecognised value is supplied.
    /// </summary>
    public string Priority { get; set; } = "Medium";

    /// <summary>Gets or sets any preliminary notes for the inspection.</summary>
    public string Notes { get; set; } = string.Empty;
}
