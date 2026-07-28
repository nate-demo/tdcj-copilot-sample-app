using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FacilityInspectionTracker.Infrastructure.Data;

namespace FacilityInspectionTracker.Infrastructure.Repositories;

public class FacilityRepository : IFacilityRepository
{
    // Technical Debt: In-memory store; connection string is hardcoded elsewhere
    private readonly List<Facility> _facilities;

    public FacilityRepository()
    {
        _facilities = SeedData.GetFacilities();
    }

    public Task<IEnumerable<Facility>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Facility>>(_facilities.Where(f => f.IsActive).ToList());
    }

    public Task<Facility?> GetByIdAsync(int id)
    {
        var facility = _facilities.FirstOrDefault(f => f.Id == id);
        return Task.FromResult(facility);
    }

    public Task<Facility> CreateAsync(Facility facility)
    {
        facility.Id = _facilities.Max(f => f.Id) + 1;
        facility.CreatedAt = DateTime.UtcNow;
        _facilities.Add(facility);
        return Task.FromResult(facility);
    }

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

    public Task<bool> DeleteAsync(int id)
    {
        var facility = _facilities.FirstOrDefault(f => f.Id == id);
        if (facility is null) return Task.FromResult(false);
        facility.IsActive = false;
        return Task.FromResult(true);
    }
}
