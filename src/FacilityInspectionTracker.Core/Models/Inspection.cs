namespace FacilityInspectionTracker.Core.Models;

public class Inspection
{
    public int Id { get; set; }
    public int FacilityId { get; set; }
    public string InspectorName { get; set; } = string.Empty;
    public string InspectionType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public InspectionStatus Status { get; set; }
    public InspectionPriority Priority { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int? Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public Facility? Facility { get; set; }
}

public enum InspectionStatus
{
    Scheduled,
    InProgress,
    Completed,
    Cancelled
}
