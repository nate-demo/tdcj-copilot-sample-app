using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FacilityInspectionTracker.Infrastructure.Data;

namespace FacilityInspectionTracker.Infrastructure.Repositories;

public class InspectionRepository : IInspectionRepository
{
    private readonly List<Inspection> _inspections;

    public InspectionRepository()
    {
        _inspections = SeedData.GetInspections();
    }

    public Task<IEnumerable<Inspection>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Inspection>>(_inspections.ToList());
    }

    public Task<IEnumerable<Inspection>> GetByFacilityIdAsync(int facilityId)
    {
        var results = _inspections.Where(i => i.FacilityId == facilityId).ToList();
        return Task.FromResult<IEnumerable<Inspection>>(results);
    }

    public Task<Inspection?> GetByIdAsync(int id)
    {
        var inspection = _inspections.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(inspection);
    }

    public Task<Inspection> CreateAsync(Inspection inspection)
    {
        inspection.Id = _inspections.Count > 0 ? _inspections.Max(i => i.Id) + 1 : 1;
        inspection.CreatedAt = DateTime.UtcNow;
        inspection.Status = InspectionStatus.Scheduled;
        _inspections.Add(inspection);
        return Task.FromResult(inspection);
    }

    public Task<Inspection?> UpdateAsync(Inspection inspection)
    {
        var existing = _inspections.FirstOrDefault(i => i.Id == inspection.Id);
        if (existing is null) return Task.FromResult<Inspection?>(null);

        existing.InspectorName = inspection.InspectorName;
        existing.InspectionType = inspection.InspectionType;
        existing.ScheduledDate = inspection.ScheduledDate;
        existing.CompletedDate = inspection.CompletedDate;
        existing.Status = inspection.Status;
        existing.Priority = inspection.Priority;
        existing.Notes = inspection.Notes;
        existing.Score = inspection.Score;
        return Task.FromResult<Inspection?>(existing);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var inspection = _inspections.FirstOrDefault(i => i.Id == id);
        if (inspection is null) return Task.FromResult(false);
        _inspections.Remove(inspection);
        return Task.FromResult(true);
    }
}
