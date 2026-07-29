using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Core.Interfaces;

/// <summary>
/// Defines the data-access contract for <see cref="Facility"/> persistence.
/// </summary>
public interface IFacilityRepository
{
    /// <summary>Returns all facilities in the data store.</summary>
    Task<IEnumerable<Facility>> GetAllAsync();

    /// <summary>
    /// Returns the facility with the specified <paramref name="id"/>,
    /// or <c>null</c> if no match is found.
    /// </summary>
    /// <param name="id">The facility's unique identifier.</param>
    Task<Facility?> GetByIdAsync(int id);

    /// <summary>Persists a new facility and returns the created record with its assigned ID.</summary>
    /// <param name="facility">The facility to create.</param>
    Task<Facility> CreateAsync(Facility facility);

    /// <summary>
    /// Applies changes from <paramref name="facility"/> to the existing record and returns the
    /// updated entity, or <c>null</c> if the facility was not found.
    /// </summary>
    /// <param name="facility">The facility containing updated values.</param>
    Task<Facility?> UpdateAsync(Facility facility);

    /// <summary>
    /// Soft-deletes the facility with the specified <paramref name="id"/>.
    /// Returns <c>true</c> if the facility was found and deleted; otherwise <c>false</c>.
    /// </summary>
    /// <param name="id">The facility's unique identifier.</param>
    Task<bool> DeleteAsync(int id);
}
