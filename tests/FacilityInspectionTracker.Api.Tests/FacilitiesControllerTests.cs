using System.Net;
using System.Net.Http.Json;
using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace FacilityInspectionTracker.Api.Tests;

/// <summary>
/// Integration tests for the FacilitiesController API endpoints.
///
/// Endpoints covered:
///   GET  /api/facilities               — retrieves all active facilities
///   GET  /api/facilities/{id}          — retrieves a facility by ID (200 or 404)
///   GET  /api/facilities/{id}/inspection-report — retrieves inspection report (200 or 404)
/// </summary>
public class FacilitiesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IFacilityService> _mockFacilityService;
    private readonly HttpClient _client;

    public FacilitiesControllerTests(WebApplicationFactory<Program> factory)
    {
        _mockFacilityService = new Mock<IFacilityService>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IFacilityService>();
                services.AddScoped<IFacilityService>(_ => _mockFacilityService.Object);
            });
        }).CreateClient();
    }

    // ── GET /api/facilities ───────────────────────────────────────────────────

    /// <summary>
    /// GET /api/facilities
    /// Returns 200 OK with a non-empty list of active facilities.
    /// </summary>
    [Fact]
    public async Task GetAll_ReturnsOk_WithFacilityList()
    {
        // Arrange
        var facilities = new List<Facility>
        {
            new Facility { Id = 1, Name = "Huntsville Unit", IsActive = true },
            new Facility { Id = 2, Name = "Darrington Unit", IsActive = true }
        };
        _mockFacilityService.Setup(s => s.GetAllFacilitiesAsync()).ReturnsAsync(facilities);

        // Act – HTTP GET /api/facilities
        var response = await _client.GetAsync("/api/facilities");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<Facility>>();
        result.Should().HaveCount(2);
        result![0].Name.Should().Be("Huntsville Unit");
    }

    /// <summary>
    /// GET /api/facilities
    /// Returns 200 OK with an empty array when no facilities exist.
    /// </summary>
    [Fact]
    public async Task GetAll_ReturnsOk_WithEmptyList_WhenNoFacilitiesExist()
    {
        // Arrange
        _mockFacilityService.Setup(s => s.GetAllFacilitiesAsync())
            .ReturnsAsync(new List<Facility>());

        // Act – HTTP GET /api/facilities
        var response = await _client.GetAsync("/api/facilities");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<Facility>>();
        result.Should().BeEmpty();
    }

    // ── GET /api/facilities/{id} ──────────────────────────────────────────────

    /// <summary>
    /// GET /api/facilities/1
    /// Returns 200 OK with the matching facility when it exists.
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsOk_WhenFacilityExists()
    {
        // Arrange
        var facility = new Facility { Id = 1, Name = "Huntsville Unit", IsActive = true };
        _mockFacilityService.Setup(s => s.GetFacilityByIdAsync(1)).ReturnsAsync(facility);

        // Act – HTTP GET /api/facilities/1
        var response = await _client.GetAsync("/api/facilities/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<Facility>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Huntsville Unit");
    }

    /// <summary>
    /// GET /api/facilities/999
    /// Returns 404 Not Found when the facility does not exist.
    /// </summary>
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenFacilityDoesNotExist()
    {
        // Arrange
        _mockFacilityService.Setup(s => s.GetFacilityByIdAsync(999))
            .ReturnsAsync((Facility?)null);

        // Act – HTTP GET /api/facilities/999
        var response = await _client.GetAsync("/api/facilities/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── GET /api/facilities/{id}/inspection-report ────────────────────────────

    /// <summary>
    /// GET /api/facilities/1/inspection-report
    /// Returns 200 OK with the inspection report when the facility exists.
    /// </summary>
    [Fact]
    public async Task GetInspectionReport_ReturnsOk_WhenFacilityExists()
    {
        // Arrange
        var report = new InspectionReport
        {
            FacilityId = 1,
            FacilityName = "Huntsville Unit",
            City = "Huntsville",
            FacilityType = "Maximum Security",
            TotalInspections = 3,
            CompletedInspections = 2,
            AverageScore = 85.0
        };
        _mockFacilityService.Setup(s => s.GetInspectionReportAsync(1)).ReturnsAsync(report);

        // Act – HTTP GET /api/facilities/1/inspection-report
        var response = await _client.GetAsync("/api/facilities/1/inspection-report");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<InspectionReport>();
        result.Should().NotBeNull();
        result!.FacilityId.Should().Be(1);
        result.FacilityName.Should().Be("Huntsville Unit");
        result.TotalInspections.Should().Be(3);
        result.CompletedInspections.Should().Be(2);
        result.AverageScore.Should().BeApproximately(85.0, 0.001);
    }

    /// <summary>
    /// GET /api/facilities/999/inspection-report
    /// Returns 404 Not Found when the facility does not exist.
    /// </summary>
    [Fact]
    public async Task GetInspectionReport_ReturnsNotFound_WhenFacilityDoesNotExist()
    {
        // Arrange
        _mockFacilityService.Setup(s => s.GetInspectionReportAsync(999))
            .ReturnsAsync((InspectionReport?)null);

        // Act – HTTP GET /api/facilities/999/inspection-report
        var response = await _client.GetAsync("/api/facilities/999/inspection-report");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
