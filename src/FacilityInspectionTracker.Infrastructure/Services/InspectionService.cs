using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Infrastructure.Services;

public class InspectionService : IInspectionService
{
    private readonly IInspectionRepository _inspectionRepository;
    private readonly IFacilityRepository _facilityRepository;

    // Technical Debt: Config value (max inspections per facility) embedded in source code
    private const int MaxInspectionsPerFacility = 20;

    public InspectionService(IInspectionRepository inspectionRepository, IFacilityRepository facilityRepository)
    {
        _inspectionRepository = inspectionRepository;
        _facilityRepository = facilityRepository;
    }

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
