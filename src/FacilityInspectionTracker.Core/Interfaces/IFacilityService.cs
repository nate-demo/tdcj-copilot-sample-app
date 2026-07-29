using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Core.Interfaces;

/// <summary>
/// Defines the business-logic contract for facility-related operations.
/// </summary>
public interface IFacilityService
{
    /// <summary>
    /// Returns all active facilities, capped at the configured maximum.
    /// </summary>
    Task<IEnumerable<Facility>> GetAllFacilitiesAsync();

    /// <summary>
    /// Returns the facility with the specified <paramref name="id"/>,
    /// or <c>null</c> if no active facility with that ID exists.
    /// </summary>
    /// <param name="id">The facility's unique identifier.</param>
    Task<Facility?> GetFacilityByIdAsync(int id);
}
