using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Infrastructure.Services;

/// <summary>
/// Provides business-logic operations for managing <see cref="Inspection"/> records.
/// </summary>
public class InspectionService : IInspectionService
{
    private readonly IInspectionRepository _inspectionRepository;
    private readonly IFacilityRepository _facilityRepository;

    // Technical Debt: Config value (max inspections per facility) embedded in source code
    private const int MaxInspectionsPerFacility = 20;

    /// <summary>
    /// Initialises a new instance of <see cref="InspectionService"/> with the required repositories.
    /// </summary>
    /// <param name="inspectionRepository">The repository used for inspection data access.</param>
    /// <param name="facilityRepository">The repository used to look up facility information.</param>
    public InspectionService(IInspectionRepository inspectionRepository, IFacilityRepository facilityRepository)
    {
        _inspectionRepository = inspectionRepository;
        _facilityRepository = facilityRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Inspection>> GetAllInspectionsAsync()
    {
        // Technical Debt: Weak exception handling
        try
        {
            return await _inspectionRepository.GetAllAsync();
        }
        catch (Exception)
        {
            return Enumerable.Empty<Inspection>();
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Inspection>> GetInspectionsByFacilityAsync(int facilityId)
    {
        try
        {
            // Technical Debt: Duplicate business logic - active facility check also in FacilityService
            var facility = await _facilityRepository.GetByIdAsync(facilityId);
            if (facility == null || !facility.IsActive)
            {
                return Enumerable.Empty<Inspection>();
            }

            var inspections = await _inspectionRepository.GetByFacilityIdAsync(facilityId);

            // Duplicate limit logic from FacilityService pattern
            var inspectionList = inspections.ToList();
            if (inspectionList.Count > MaxInspectionsPerFacility)
            {
                inspectionList = inspectionList.Take(MaxInspectionsPerFacility).ToList();
            }

            return inspectionList;
        }
        catch (Exception)
        {
            return Enumerable.Empty<Inspection>();
        }
    }

    /// <inheritdoc />
    public async Task<Inspection?> GetInspectionByIdAsync(int id)
    {
        try
        {
            return await _inspectionRepository.GetByIdAsync(id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<Inspection> ScheduleInspectionAsync(Inspection inspection)
    {
        // Technical Debt: Missing validation for inspection date - no check that date is in the future
        // or that the facility exists before scheduling

        var facility = await _facilityRepository.GetByIdAsync(inspection.FacilityId);
        if (facility == null)
        {
            throw new Exception("Facility not found");
        }

        inspection.Status = InspectionStatus.Scheduled;
        return await _inspectionRepository.CreateAsync(inspection);
    }

    /// <inheritdoc />
    public async Task<bool> CancelInspectionAsync(int id)
    {
        try
        {
            var inspection = await _inspectionRepository.GetByIdAsync(id);
            if (inspection == null) return false;

            if (inspection.Status == InspectionStatus.Completed)
            {
                throw new Exception("Cannot cancel a completed inspection");
            }

            inspection.Status = InspectionStatus.Cancelled;
            await _inspectionRepository.UpdateAsync(inspection);
            return true;
        }
        catch (Exception)
        {
            // Technical Debt: Swallowing exception, caller won't know reason for failure
            return false;
        }
    }
}
