using System.Net;
using System.Net.Http.Json;
using FacilityInspectionTracker.Api.Models;
using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace FacilityInspectionTracker.Api.Tests;

/// <summary>
/// Integration tests for the InspectionsController API endpoints.
///
/// Endpoints covered:
///   GET    /api/inspections                      — retrieves all inspections
///   GET    /api/inspections/facility/{facilityId} — retrieves inspections for a facility
///   GET    /api/inspections/{id}                 — retrieves an inspection by ID (200 or 404)
///   POST   /api/inspections                      — schedules a new inspection (201 or 400)
///   DELETE /api/inspections/{id}/cancel          — cancels an inspection (204 or 404)
/// </summary>
public class InspectionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IInspectionService> _mockInspectionService;
    private readonly HttpClient _client;

    public InspectionsControllerTests(WebApplicationFactory<Program> factory)
    {
        _mockInspectionService = new Mock<IInspectionService>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IInspectionService>();
                services.AddScoped<IInspectionService>(_ => _mockInspectionService.Object);
            });
        }).CreateClient();
    }

    // ── GET /api/inspections ─────────────────────────────────────────────────

    /// <summary>
    /// GET /api/inspections
    /// Returns 200 OK with a list of all inspections.
    /// </summary>
    [Fact]
    public async Task GetAll_ReturnsOk_WithInspectionList()
    {
        // Arrange
        var inspections = new List<Inspection>
        {
            new Inspection { Id = 1, FacilityId = 1, InspectorName = "James Holden", Status = InspectionStatus.Completed },
            new Inspection { Id = 2, FacilityId = 1, InspectorName = "Maria Santos", Status = InspectionStatus.Scheduled }
        };
        _mockInspectionService.Setup(s => s.GetAllInspectionsAsync()).ReturnsAsync(inspections);

        // Act – HTTP GET /api/inspections
        var response = await _client.GetAsync("/api/inspections");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<Inspection>>();
        result.Should().HaveCount(2);
        result![0].InspectorName.Should().Be("James Holden");
    }

    /// <summary>
    /// GET /api/inspections
    /// Returns 200 OK with an empty array when no inspections exist.
    /// </summary>
    [Fact]
    public async Task GetAll_ReturnsOk_WithEmptyList_WhenNoInspectionsExist()
    {
        // Arrange
        _mockInspectionService.Setup(s => s.GetAllInspectionsAsync())
            .ReturnsAsync(new List<Inspection>());

        // Act – HTTP GET /api/inspections
        var response = await _client.GetAsync("/api/inspections");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<Inspection>>();
        result.Should().BeEmpty();
    }

    // ── GET /api/inspections/facility/{facilityId} ───────────────────────────

    /// <summary>
    /// GET /api/inspections/facility/1
    /// Returns 200 OK with inspections belonging to facility 1.
    /// </summary>
    [Fact]
    public async Task GetByFacility_ReturnsOk_WithInspectionsForFacility()
    {
        // Arrange
        var inspections = new List<Inspection>
        {
            new Inspection { Id = 1, FacilityId = 1, InspectorName = "James Holden", Status = InspectionStatus.Completed },
            new Inspection { Id = 2, FacilityId = 1, InspectorName = "Maria Santos", Status = InspectionStatus.Scheduled }
        };
        _mockInspectionService.Setup(s => s.GetInspectionsByFacilityAsync(1)).ReturnsAsync(inspections);

        // Act – HTTP GET /api/inspections/facility/1
        var response = await _client.GetAsync("/api/inspections/facility/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<Inspection>>();
        result.Should().HaveCount(2);
        result!.All(i => i.FacilityId == 1).Should().BeTrue();
    }

    /// <summary>
    /// GET /api/inspections/facility/999
    /// Returns 200 OK with an empty array when the facility has no inspections.
    /// </summary>
    [Fact]
    public async Task GetByFacility_ReturnsOk_WithEmptyList_WhenFacilityHasNoInspections()
    {
        // Arrange
        _mockInspectionService.Setup(s => s.GetInspectionsByFacilityAsync(999))
            .ReturnsAsync(new List<Inspection>());

        // Act – HTTP GET /api/inspections/facility/999
        var response = await _client.GetAsync("/api/inspections/facility/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<Inspection>>();
        result.Should().BeEmpty();
    }

    // ── GET /api/inspections/{id} ────────────────────────────────────────────

    /// <summary>
    /// GET /api/inspections/1
    /// Returns 200 OK with the matching inspection when it exists.
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsOk_WhenInspectionExists()
    {
        // Arrange
        var inspection = new Inspection
        {
            Id = 1,
            FacilityId = 1,
            InspectorName = "James Holden",
            InspectionType = "Fire Safety",
            Status = InspectionStatus.Completed,
            Priority = InspectionPriority.High
        };
        _mockInspectionService.Setup(s => s.GetInspectionByIdAsync(1)).ReturnsAsync(inspection);

        // Act – HTTP GET /api/inspections/1
        var response = await _client.GetAsync("/api/inspections/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<Inspection>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.InspectorName.Should().Be("James Holden");
        result.InspectionType.Should().Be("Fire Safety");
    }

    /// <summary>
    /// GET /api/inspections/999
    /// Returns 404 Not Found when the inspection does not exist.
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenInspectionDoesNotExist()
    {
        // Arrange
        _mockInspectionService.Setup(s => s.GetInspectionByIdAsync(999))
            .ReturnsAsync((Inspection?)null);

        // Act – HTTP GET /api/inspections/999
        var response = await _client.GetAsync("/api/inspections/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── POST /api/inspections ────────────────────────────────────────────────

    /// <summary>
    /// POST /api/inspections
    /// Returns 201 Created with the new inspection when the request is valid.
    /// Body: { facilityId, inspectorName, inspectionType, scheduledDate, priority, notes }
    /// </summary>
    [Fact]
    public async Task Schedule_ReturnsCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new ScheduleInspectionRequest
        {
            FacilityId = 1,
            InspectorName = "Jane Smith",
            InspectionType = "Fire Safety",
            ScheduledDate = DateTime.Now.AddDays(7),
            Priority = "High",
            Notes = "Annual fire safety review"
        };

        var createdInspection = new Inspection
        {
            Id = 10,
            FacilityId = request.FacilityId,
            InspectorName = request.InspectorName,
            InspectionType = request.InspectionType,
            ScheduledDate = request.ScheduledDate,
            Priority = InspectionPriority.High,
            Notes = request.Notes,
            Status = InspectionStatus.Scheduled
        };

        _mockInspectionService.Setup(s => s.ScheduleInspectionAsync(It.IsAny<Inspection>()))
            .ReturnsAsync(createdInspection);

        // Act – HTTP POST /api/inspections
        var response = await _client.PostAsJsonAsync("/api/inspections", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var result = await response.Content.ReadFromJsonAsync<Inspection>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(10);
        result.InspectorName.Should().Be("Jane Smith");
        result.InspectionType.Should().Be("Fire Safety");
    }

    /// <summary>
    /// POST /api/inspections
    /// Returns 400 Bad Request when the service throws (e.g., facility not found).
    /// </summary>
    [Fact]
    public async Task Schedule_ReturnsBadRequest_WhenFacilityDoesNotExist()
    {
        // Arrange
        var request = new ScheduleInspectionRequest
        {
            FacilityId = 999,
            InspectorName = "Jane Smith",
            InspectionType = "Fire Safety",
            ScheduledDate = DateTime.Now.AddDays(7),
            Priority = "High"
        };

        _mockInspectionService.Setup(s => s.ScheduleInspectionAsync(It.IsAny<Inspection>()))
            .ThrowsAsync(new Exception("Facility not found"));

        // Act – HTTP POST /api/inspections
        var response = await _client.PostAsJsonAsync("/api/inspections", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// POST /api/inspections
    /// Priority defaults to Medium when the provided priority string is unrecognised.
    /// </summary>
    [Fact]
    public async Task Schedule_DefaultsPriorityToMedium_WhenPriorityIsInvalid()
    {
        // Arrange
        var request = new ScheduleInspectionRequest
        {
            FacilityId = 1,
            InspectorName = "Jane Smith",
            InspectionType = "Fire Safety",
            ScheduledDate = DateTime.Now.AddDays(7),
            Priority = "UNKNOWN"
        };

        Inspection? capturedInspection = null;
        _mockInspectionService
            .Setup(s => s.ScheduleInspectionAsync(It.IsAny<Inspection>()))
            .Callback<Inspection>(i => capturedInspection = i)
            .ReturnsAsync(new Inspection { Id = 11, FacilityId = 1, Status = InspectionStatus.Scheduled });

        // Act – HTTP POST /api/inspections
        var response = await _client.PostAsJsonAsync("/api/inspections", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        capturedInspection.Should().NotBeNull();
        capturedInspection!.Priority.Should().Be(InspectionPriority.Medium);
    }

    // ── DELETE /api/inspections/{id}/cancel ──────────────────────────────────

    /// <summary>
    /// DELETE /api/inspections/2/cancel
    /// Returns 204 No Content when the inspection is successfully cancelled.
    /// </summary>
    [Fact]
    public async Task Cancel_ReturnsNoContent_WhenInspectionIsCancelled()
    {
        // Arrange
        _mockInspectionService.Setup(s => s.CancelInspectionAsync(2)).ReturnsAsync(true);

        // Act – HTTP DELETE /api/inspections/2/cancel
        var response = await _client.DeleteAsync("/api/inspections/2/cancel");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// DELETE /api/inspections/999/cancel
    /// Returns 404 Not Found when the inspection does not exist or cannot be cancelled.
    /// </summary>
    [Fact]
    public async Task Cancel_ReturnsNotFound_WhenInspectionDoesNotExist()
    {
        // Arrange
        _mockInspectionService.Setup(s => s.CancelInspectionAsync(999)).ReturnsAsync(false);

        // Act – HTTP DELETE /api/inspections/999/cancel
        var response = await _client.DeleteAsync("/api/inspections/999/cancel");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
