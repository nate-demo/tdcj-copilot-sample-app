namespace FacilityInspectionTracker.Core.Models;

public class InspectionReport
{
    public int FacilityId { get; set; }
    public string FacilityName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string FacilityType { get; set; } = string.Empty;
    public int TotalInspections { get; set; }
    public int CompletedInspections { get; set; }
    public double? AverageScore { get; set; }
    public DateTime? MostRecentInspectionDate { get; set; }
    public List<Inspection> Inspections { get; set; } = new();
}
