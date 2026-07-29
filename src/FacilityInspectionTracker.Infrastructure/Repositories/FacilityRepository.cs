using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FacilityInspectionTracker.Infrastructure.Data;

namespace FacilityInspectionTracker.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of <see cref="IFacilityRepository"/>.
/// Data is seeded from <see cref="SeedData"/> at startup and persists for the lifetime
/// of the singleton instance.
/// </summary>
public class FacilityRepository : IFacilityRepository
{
    // Technical Debt: In-memory store; connection string is hardcoded elsewhere
    private readonly List<Facility> _facilities;

    /// <summary>
    /// Initialises a new instance of <see cref="FacilityRepository"/> and seeds it with
    /// sample facility data.
    /// </summary>
    public FacilityRepository()
    {
        _facilities = SeedData.GetFacilities();
    }

    /// <inheritdoc />
    public Task<IEnumerable<Facility>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Facility>>(_facilities.Where(f => f.IsActive).ToList());
    }

    /// <inheritdoc />
    public Task<Facility?> GetByIdAsync(int id)
    {
        var facility = _facilities.FirstOrDefault(f => f.Id == id);
        return Task.FromResult(facility);
    }

    /// <inheritdoc />
    public Task<Facility> CreateAsync(Facility facility)
    {
        facility.Id = _facilities.Max(f => f.Id) + 1;
        facility.CreatedAt = DateTime.UtcNow;
        _facilities.Add(facility);
        return Task.FromResult(facility);
    }

    /// <inheritdoc />
    public Task<Facility?> UpdateAsync(Facility facility)
    {
        var existing = _facilities.FirstOrDefault(f => f.Id == facility.Id);
        if (existing is null) return Task.FromResult<Facility?>(null);

        existing.Name = facility.Name;
        existing.Address = facility.Address;
        existing.City = facility.City;
        existing.State = facility.State;
        existing.ZipCode = facility.ZipCode;
        existing.FacilityType = facility.FacilityType;
        existing.Capacity = facility.Capacity;
        existing.IsActive = facility.IsActive;
        return Task.FromResult<Facility?>(existing);
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(int id)
    {
        var facility = _facilities.FirstOrDefault(f => f.Id == id);
        if (facility is null) return Task.FromResult(false);
        facility.IsActive = false;
        return Task.FromResult(true);
    }
}
