using FacilityInspectionTracker.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FacilityInspectionTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacilitiesController : ControllerBase
{
    private readonly IFacilityService _facilityService;

    public FacilitiesController(IFacilityService facilityService)
    {
        _facilityService = facilityService;
    }

    /// <summary>
    /// Retrieves all active facilities.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var facilities = await _facilityService.GetAllFacilitiesAsync();
        return Ok(facilities);
    }

    /// <summary>
    /// Retrieves a specific facility by its identifier.
    /// </summary>
    /// <param name="id">The facility ID.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var facility = await _facilityService.GetFacilityByIdAsync(id);
        if (facility is null)
            return NotFound(new { message = $"Facility {id} not found." });

        return Ok(facility);
    }

    /// <summary>
    /// Retrieves an inspection report for a specific facility.
    /// </summary>
    /// <param name="id">The facility ID.</param>
    [HttpGet("{id:int}/inspection-report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInspectionReport(int id)
    {
        var report = await _facilityService.GetInspectionReportAsync(id);
        if (report is null)
            return NotFound(new { message = $"Facility {id} not found." });

        return Ok(report);
    }
}
