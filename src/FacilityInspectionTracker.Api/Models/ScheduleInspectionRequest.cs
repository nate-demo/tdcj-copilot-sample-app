namespace FacilityInspectionTracker.Api.Models;

public class ScheduleInspectionRequest
{
    public int FacilityId { get; set; }
    public string InspectorName { get; set; } = string.Empty;
    public string InspectionType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Priority { get; set; } = "Medium";
    public string Notes { get; set; } = string.Empty;
}
