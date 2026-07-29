using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Infrastructure.Services;

/// <summary>
/// Provides business-logic operations for managing <see cref="Facility"/> records.
/// </summary>
public class FacilityService : IFacilityService
{
    private readonly IFacilityRepository _facilityRepository;

    // Technical Debt: Config value (max facilities limit) embedded in source code
    private const int MaxActiveFacilities = 50;

    /// <summary>
    /// Initialises a new instance of <see cref="FacilityService"/> with the required repository.
    /// </summary>
    /// <param name="facilityRepository">The repository used for facility data access.</param>
    public FacilityService(IFacilityRepository facilityRepository)
    {
        _facilityRepository = facilityRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Facility>> GetAllFacilitiesAsync()
    {
        // Technical Debt: Weak exception handling - swallowing exception details
        try
        {
            var facilities = await _facilityRepository.GetAllAsync();

            // Technical Debt: Duplicate business logic - also exists in InspectionService
            var activeFacilities = facilities.Where(f => f.IsActive).ToList();
            if (activeFacilities.Count > MaxActiveFacilities)
            {
                activeFacilities = activeFacilities.Take(MaxActiveFacilities).ToList();
            }

            return activeFacilities;
        }
        catch (Exception)
        {
            // swallowing the exception - returning empty list silently
            return Enumerable.Empty<Facility>();
        }
    }

    /// <inheritdoc />
    public async Task<Facility?> GetFacilityByIdAsync(int id)
    {
        try
        {
            return await _facilityRepository.GetByIdAsync(id);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
