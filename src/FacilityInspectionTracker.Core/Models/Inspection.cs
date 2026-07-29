namespace FacilityInspectionTracker.Core.Models;

/// <summary>
/// Represents a scheduled or completed inspection of a TDCJ facility.
/// </summary>
public class Inspection
{
    /// <summary>Gets or sets the unique identifier for the inspection.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the identifier of the facility being inspected.</summary>
    public int FacilityId { get; set; }

    /// <summary>Gets or sets the full name of the inspector assigned to this inspection.</summary>
    public string InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category of the inspection (e.g., "Fire Safety", "Structural Integrity").
    /// </summary>
    public string InspectionType { get; set; } = string.Empty;

    /// <summary>Gets or sets the UTC date and time when the inspection is scheduled to occur.</summary>
    public DateTime ScheduledDate { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the inspection was completed.
    /// <c>null</c> if the inspection has not yet been completed.
    /// </summary>
    public DateTime? CompletedDate { get; set; }

    /// <summary>Gets or sets the current lifecycle status of the inspection.</summary>
    public InspectionStatus Status { get; set; }

    /// <summary>Gets or sets the urgency level that determines scheduling priority.</summary>
    public InspectionPriority Priority { get; set; }

    /// <summary>Gets or sets free-form notes recorded by the inspector.</summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the numeric score assigned upon completion (0–100).
    /// <c>null</c> if the inspection has not been scored.
    /// </summary>
    public int? Score { get; set; }

    /// <summary>Gets or sets the UTC date and time when the inspection record was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the facility associated with this inspection.
    /// May be <c>null</c> when the navigation property is not loaded.
    /// </summary>
    public Facility? Facility { get; set; }
}

/// <summary>
/// Describes the current lifecycle state of an <see cref="Inspection"/>.
/// </summary>
public enum InspectionStatus
{
    /// <summary>The inspection is booked but has not yet taken place.</summary>
    Scheduled,

    /// <summary>The inspection is actively being conducted.</summary>
    InProgress,

    /// <summary>The inspection has been finished and a score has been recorded.</summary>
    Completed,

    /// <summary>The inspection was cancelled before it could be completed.</summary>
    Cancelled
}
