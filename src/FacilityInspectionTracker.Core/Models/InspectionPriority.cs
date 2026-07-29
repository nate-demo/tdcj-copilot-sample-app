namespace FacilityInspectionTracker.Core.Models;

/// <summary>
/// Represents the urgency level assigned to an <see cref="Inspection"/>.
/// Higher numeric values indicate greater urgency.
/// </summary>
public enum InspectionPriority
{
    /// <summary>Routine inspection with no time-sensitive concerns.</summary>
    Low = 1,

    /// <summary>Standard priority; should be completed within the normal scheduling window.</summary>
    Medium = 2,

    /// <summary>Elevated priority due to identified risks or compliance requirements.</summary>
    High = 3,

    /// <summary>Immediate attention required; potential safety or legal risk.</summary>
    Critical = 4
}
