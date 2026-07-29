using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Core.Interfaces;

/// <summary>
/// Defines the business-logic contract for inspection-related operations.
/// </summary>
public interface IInspectionService
{
    /// <summary>Returns all inspections across every facility.</summary>
    Task<IEnumerable<Inspection>> GetAllInspectionsAsync();

    /// <summary>
    /// Returns all inspections for the specified facility.
    /// Returns an empty collection if the facility is inactive or does not exist.
    /// </summary>
    /// <param name="facilityId">The facility's unique identifier.</param>
    Task<IEnumerable<Inspection>> GetInspectionsByFacilityAsync(int facilityId);

    /// <summary>
    /// Returns the inspection with the specified <paramref name="id"/>,
    /// or <c>null</c> if no match is found.
    /// </summary>
    /// <param name="id">The inspection's unique identifier.</param>
    Task<Inspection?> GetInspectionByIdAsync(int id);

    /// <summary>
    /// Schedules a new inspection and returns the persisted record.
    /// </summary>
    /// <param name="inspection">
    /// The inspection to schedule. <see cref="Inspection.FacilityId"/> must reference
    /// an existing facility.
    /// </param>
    /// <exception cref="Exception">Thrown when the referenced facility does not exist.</exception>
    Task<Inspection> ScheduleInspectionAsync(Inspection inspection);

    /// <summary>
    /// Cancels the inspection with the specified <paramref name="id"/>.
    /// Returns <c>true</c> on success, or <c>false</c> if the inspection was not found
    /// or could not be cancelled.
    /// </summary>
    /// <param name="id">The inspection's unique identifier.</param>
    Task<bool> CancelInspectionAsync(int id);
}
