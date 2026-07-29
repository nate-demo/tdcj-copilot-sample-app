using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Core.Interfaces;

/// <summary>
/// Defines the data-access contract for <see cref="Inspection"/> persistence.
/// </summary>
public interface IInspectionRepository
{
    /// <summary>Returns all inspections in the data store.</summary>
    Task<IEnumerable<Inspection>> GetAllAsync();

    /// <summary>Returns all inspections associated with the specified facility.</summary>
    /// <param name="facilityId">The facility's unique identifier.</param>
    Task<IEnumerable<Inspection>> GetByFacilityIdAsync(int facilityId);

    /// <summary>
    /// Returns the inspection with the specified <paramref name="id"/>,
    /// or <c>null</c> if no match is found.
    /// </summary>
    /// <param name="id">The inspection's unique identifier.</param>
    Task<Inspection?> GetByIdAsync(int id);

    /// <summary>Persists a new inspection and returns the created record with its assigned ID.</summary>
    /// <param name="inspection">The inspection to create.</param>
    Task<Inspection> CreateAsync(Inspection inspection);

    /// <summary>
    /// Applies changes from <paramref name="inspection"/> to the existing record and returns the
    /// updated entity, or <c>null</c> if the inspection was not found.
    /// </summary>
    /// <param name="inspection">The inspection containing updated values.</param>
    Task<Inspection?> UpdateAsync(Inspection inspection);

    /// <summary>
    /// Removes the inspection with the specified <paramref name="id"/> from the data store.
    /// Returns <c>true</c> if the inspection was found and deleted; otherwise <c>false</c>.
    /// </summary>
    /// <param name="id">The inspection's unique identifier.</param>
    Task<bool> DeleteAsync(int id);
}
