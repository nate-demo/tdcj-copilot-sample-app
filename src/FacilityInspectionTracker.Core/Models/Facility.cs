namespace FacilityInspectionTracker.Core.Models;

/// <summary>
/// Represents a TDCJ facility that can be subject to inspections.
/// </summary>
public class Facility
{
    /// <summary>Gets or sets the unique identifier for the facility.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the name of the facility.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the street address of the facility.</summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>Gets or sets the city where the facility is located.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Gets or sets the two-letter state abbreviation where the facility is located.</summary>
    public string State { get; set; } = string.Empty;

    /// <summary>Gets or sets the ZIP code for the facility's address.</summary>
    public string ZipCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the classification of the facility (e.g., "Maximum Security", "Medium Security").
    /// </summary>
    public string FacilityType { get; set; } = string.Empty;

    /// <summary>Gets or sets the rated inmate capacity of the facility.</summary>
    public int Capacity { get; set; }

    /// <summary>Gets or sets a value indicating whether the facility is currently operational.</summary>
    public bool IsActive { get; set; }

    /// <summary>Gets or sets the UTC date and time when the facility record was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the collection of inspections associated with this facility.</summary>
    public List<Inspection> Inspections { get; set; } = new();
}
