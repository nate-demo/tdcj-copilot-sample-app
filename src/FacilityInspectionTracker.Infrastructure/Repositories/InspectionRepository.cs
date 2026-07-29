using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FacilityInspectionTracker.Infrastructure.Data;

namespace FacilityInspectionTracker.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of <see cref="IInspectionRepository"/>.
/// Data is seeded from <see cref="SeedData"/> at startup and persists for the lifetime
/// of the singleton instance.
/// </summary>
public class InspectionRepository : IInspectionRepository
{
    private readonly List<Inspection> _inspections;

    /// <summary>
    /// Initialises a new instance of <see cref="InspectionRepository"/> and seeds it with
    /// sample inspection data.
    /// </summary>
    public InspectionRepository()
    {
        _inspections = SeedData.GetInspections();
    }

    /// <inheritdoc />
    public Task<IEnumerable<Inspection>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Inspection>>(_inspections.ToList());
    }

    /// <inheritdoc />
    public Task<IEnumerable<Inspection>> GetByFacilityIdAsync(int facilityId)
    {
        var results = _inspections.Where(i => i.FacilityId == facilityId).ToList();
        return Task.FromResult<IEnumerable<Inspection>>(results);
    }

    /// <inheritdoc />
    public Task<Inspection?> GetByIdAsync(int id)
    {
        var inspection = _inspections.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(inspection);
    }

    /// <inheritdoc />
    public Task<Inspection> CreateAsync(Inspection inspection)
    {
        inspection.Id = _inspections.Count > 0 ? _inspections.Max(i => i.Id) + 1 : 1;
        inspection.CreatedAt = DateTime.UtcNow;
        inspection.Status = InspectionStatus.Scheduled;
        _inspections.Add(inspection);
        return Task.FromResult(inspection);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public Task<bool> DeleteAsync(int id)
    {
        var inspection = _inspections.FirstOrDefault(i => i.Id == id);
        if (inspection is null) return Task.FromResult(false);
        _inspections.Remove(inspection);
        return Task.FromResult(true);
    }
}
