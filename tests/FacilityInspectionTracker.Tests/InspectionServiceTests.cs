using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FacilityInspectionTracker.Infrastructure.Services;
using FluentAssertions;
using Moq;

namespace FacilityInspectionTracker.Tests;

public class InspectionServiceTests
{
    private readonly Mock<IInspectionRepository> _mockInspectionRepo;
    private readonly Mock<IFacilityRepository> _mockFacilityRepo;
    private readonly InspectionService _service;

    public InspectionServiceTests()
    {
        _mockInspectionRepo = new Mock<IInspectionRepository>();
        _mockFacilityRepo = new Mock<IFacilityRepository>();
        _service = new InspectionService(_mockInspectionRepo.Object, _mockFacilityRepo.Object);
    }

    [Fact]
    public async Task GetAllInspectionsAsync_ReturnsAllInspections()
    {
        var inspections = new List<Inspection>
        {
            new Inspection { Id = 1, FacilityId = 1, Status = InspectionStatus.Scheduled },
            new Inspection { Id = 2, FacilityId = 2, Status = InspectionStatus.Completed }
        };
        _mockInspectionRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(inspections);

        var result = await _service.GetAllInspectionsAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ScheduleInspectionAsync_CreatesInspection_WhenFacilityExists()
    {
        var facility = new Facility { Id = 1, Name = "Test Facility", IsActive = true };
        _mockFacilityRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(facility);

        var inspection = new Inspection
        {
            FacilityId = 1,
            InspectorName = "John Doe",
            InspectionType = "Fire Safety",
            ScheduledDate = DateTime.Now.AddDays(7),
            Priority = InspectionPriority.High
        };
        _mockInspectionRepo.Setup(r => r.CreateAsync(It.IsAny<Inspection>())).ReturnsAsync(inspection);

        var result = await _service.ScheduleInspectionAsync(inspection);

        result.Should().NotBeNull();
        result.InspectorName.Should().Be("John Doe");
    }

    [Fact]
    public async Task ScheduleInspectionAsync_ThrowsException_WhenFacilityNotFound()
    {
        _mockFacilityRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Facility?)null);

        var inspection = new Inspection { FacilityId = 999, ScheduledDate = DateTime.Now.AddDays(7) };

        var act = async () => await _service.ScheduleInspectionAsync(inspection);
        await act.Should().ThrowAsync<Exception>().WithMessage("Facility not found");
    }

    [Fact]
    public async Task CancelInspectionAsync_ReturnsFalse_WhenInspectionNotFound()
    {
        _mockInspectionRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Inspection?)null);

        var result = await _service.CancelInspectionAsync(999);

        result.Should().BeFalse();
    }

    // Technical Debt: Low test coverage - missing tests for:
    // - CancelInspectionAsync with Completed status
    // - GetInspectionsByFacilityAsync with inactive facility
    // - GetInspectionByIdAsync success/failure paths
}
