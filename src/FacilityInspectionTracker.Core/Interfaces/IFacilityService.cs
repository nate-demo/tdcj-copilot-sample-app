using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Core.Interfaces;

public interface IFacilityService
{
    Task<IEnumerable<Facility>> GetAllFacilitiesAsync();
    Task<Facility?> GetFacilityByIdAsync(int id);
    Task<InspectionReport?> GetInspectionReportAsync(int facilityId);
}
