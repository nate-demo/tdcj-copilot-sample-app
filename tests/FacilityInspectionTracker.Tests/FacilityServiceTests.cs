using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FacilityInspectionTracker.Infrastructure.Services;
using FluentAssertions;
using Moq;

namespace FacilityInspectionTracker.Tests;

public class FacilityServiceTests
{
    private readonly Mock<IFacilityRepository> _mockRepo;
    private readonly FacilityService _service;

    public FacilityServiceTests()
    {
        _mockRepo = new Mock<IFacilityRepository>();
        _service = new FacilityService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllFacilitiesAsync_ReturnsActiveFacilities()
    {
        var facilities = new List<Facility>
        {
            new Facility { Id = 1, Name = "Unit A", IsActive = true },
            new Facility { Id = 2, Name = "Unit B", IsActive = true }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(facilities);

        var result = await _service.GetAllFacilitiesAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetFacilityByIdAsync_ReturnsCorrectFacility()
    {
        var facility = new Facility { Id = 1, Name = "Huntsville Unit", IsActive = true };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(facility);

        var result = await _service.GetFacilityByIdAsync(1);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Huntsville Unit");
    }

    [Fact]
    public async Task GetFacilityByIdAsync_ReturnsNull_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Facility?)null);

        var result = await _service.GetFacilityByIdAsync(999);

        result.Should().BeNull();
    }

    // Technical Debt: Low unit-test coverage - missing tests for:
    // - GetAllFacilitiesAsync when repository throws exception
    // - GetAllFacilitiesAsync with inactive facilities
    // - GetFacilityByIdAsync when repository throws exception
}
