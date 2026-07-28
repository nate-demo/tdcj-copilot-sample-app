using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Core.Interfaces;

public interface IInspectionService
{
    Task<IEnumerable<Inspection>> GetAllInspectionsAsync();
    Task<IEnumerable<Inspection>> GetInspectionsByFacilityAsync(int facilityId);
    Task<Inspection?> GetInspectionByIdAsync(int id);
    Task<Inspection> ScheduleInspectionAsync(Inspection inspection);
    Task<bool> CancelInspectionAsync(int id);
}
