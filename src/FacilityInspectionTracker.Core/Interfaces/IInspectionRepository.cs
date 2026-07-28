using FacilityInspectionTracker.Core.Models;

namespace FacilityInspectionTracker.Core.Interfaces;

public interface IInspectionRepository
{
    Task<IEnumerable<Inspection>> GetAllAsync();
    Task<IEnumerable<Inspection>> GetByFacilityIdAsync(int facilityId);
    Task<Inspection?> GetByIdAsync(int id);
    Task<Inspection> CreateAsync(Inspection inspection);
    Task<Inspection?> UpdateAsync(Inspection inspection);
    Task<bool> DeleteAsync(int id);
}
