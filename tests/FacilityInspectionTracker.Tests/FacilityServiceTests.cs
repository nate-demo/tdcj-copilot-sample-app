using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FacilityInspectionTracker.Infrastructure.Services;
using FluentAssertions;
using Moq;

namespace FacilityInspectionTracker.Tests;

public class FacilityServiceTests
{
    private readonly Mock<IFacilityRepository> _mockRepo;
    private readonly Mock<IInspectionRepository> _mockInspectionRepo;
    private readonly FacilityService _service;

    public FacilityServiceTests()
    {
        _mockRepo = new Mock<IFacilityRepository>();
        _mockInspectionRepo = new Mock<IInspectionRepository>();
        _service = new FacilityService(_mockRepo.Object, _mockInspectionRepo.Object);
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

    [Fact]
    public async Task GetInspectionReportAsync_ReturnsNull_WhenFacilityNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Facility?)null);

        var result = await _service.GetInspectionReportAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetInspectionReportAsync_ReturnsReport_WithCorrectSummary()
    {
        var facility = new Facility
        {
            Id = 1,
            Name = "Huntsville Unit",
            City = "Huntsville",
            FacilityType = "Prison",
            IsActive = true
        };

        var inspections = new List<Inspection>
        {
            new Inspection { Id = 1, FacilityId = 1, Status = InspectionStatus.Completed, Score = 80, ScheduledDate = new DateTime(2025, 1, 10) },
            new Inspection { Id = 2, FacilityId = 1, Status = InspectionStatus.Completed, Score = 90, ScheduledDate = new DateTime(2025, 3, 15) },
            new Inspection { Id = 3, FacilityId = 1, Status = InspectionStatus.Scheduled, Score = null, ScheduledDate = new DateTime(2025, 6, 1) }
        };

        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(facility);
        _mockInspectionRepo.Setup(r => r.GetByFacilityIdAsync(1)).ReturnsAsync(inspections);

        var result = await _service.GetInspectionReportAsync(1);

        result.Should().NotBeNull();
        result!.FacilityId.Should().Be(1);
        result.FacilityName.Should().Be("Huntsville Unit");
        result.City.Should().Be("Huntsville");
        result.FacilityType.Should().Be("Prison");
        result.TotalInspections.Should().Be(3);
        result.CompletedInspections.Should().Be(2);
        result.AverageScore.Should().BeApproximately(85.0, 0.001);
        result.MostRecentInspectionDate.Should().Be(new DateTime(2025, 6, 1));
        result.Inspections.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetInspectionReportAsync_ReturnsNullAverageScore_WhenNoCompletedInspectionsHaveScores()
    {
        var facility = new Facility { Id = 2, Name = "Unit B", City = "Austin", FacilityType = "Jail", IsActive = true };
        var inspections = new List<Inspection>
        {
            new Inspection { Id = 10, FacilityId = 2, Status = InspectionStatus.Scheduled, Score = null, ScheduledDate = DateTime.Today }
        };

        _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(facility);
        _mockInspectionRepo.Setup(r => r.GetByFacilityIdAsync(2)).ReturnsAsync(inspections);

        var result = await _service.GetInspectionReportAsync(2);

        result.Should().NotBeNull();
        result!.AverageScore.Should().BeNull();
        result.CompletedInspections.Should().Be(0);
    }

    // Technical Debt: Low unit-test coverage - missing tests for:
    // - GetAllFacilitiesAsync when repository throws exception
    // - GetAllFacilitiesAsync with inactive facilities
    // - GetFacilityByIdAsync when repository throws exception
}
