using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Infrastructure.Services;

public class FacilityService : IFacilityService
{
    private readonly IFacilityRepository _facilityRepository;
    private readonly IInspectionRepository _inspectionRepository;

    // Technical Debt: Config value (max facilities limit) embedded in source code
    private const int MaxActiveFacilities = 50;

    public FacilityService(IFacilityRepository facilityRepository, IInspectionRepository inspectionRepository)
    {
        _facilityRepository = facilityRepository;
        _inspectionRepository = inspectionRepository;
    }

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

    public async Task<InspectionReport?> GetInspectionReportAsync(int facilityId)
    {
        try
        {
            var facility = await _facilityRepository.GetByIdAsync(facilityId);
            if (facility == null)
                return null;

            var inspections = (await _inspectionRepository.GetByFacilityIdAsync(facilityId)).ToList();
            var completedInspections = inspections.Where(i => i.Status == InspectionStatus.Completed).ToList();
            var scores = completedInspections.Where(i => i.Score.HasValue).Select(i => i.Score!.Value).ToList();

            return new InspectionReport
            {
                FacilityId = facility.Id,
                FacilityName = facility.Name,
                City = facility.City,
                FacilityType = facility.FacilityType,
                TotalInspections = inspections.Count,
                CompletedInspections = completedInspections.Count,
                AverageScore = scores.Count > 0 ? scores.Average() : null,
                MostRecentInspectionDate = inspections.Count > 0
                    ? inspections.Max(i => i.ScheduledDate)
                    : null,
                Inspections = inspections
            };
        }
        catch (Exception)
        {
            return null;
        }
    }
}
