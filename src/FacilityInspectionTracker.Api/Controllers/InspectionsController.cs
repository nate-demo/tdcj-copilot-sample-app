using FacilityInspectionTracker.Api.Models;
using FacilityInspectionTracker.Core.Interfaces;
using FacilityInspectionTracker.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FacilityInspectionTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InspectionsController : ControllerBase
{
    private readonly IInspectionService _inspectionService;

    public InspectionsController(IInspectionService inspectionService)
    {
        _inspectionService = inspectionService;
    }

    /// <summary>
    /// Retrieves all inspections.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var inspections = await _inspectionService.GetAllInspectionsAsync();
        return Ok(inspections);
    }

    /// <summary>
    /// Retrieves all inspections for a specific facility.
    /// </summary>
    /// <param name="facilityId">The facility ID.</param>
    [HttpGet("facility/{facilityId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByFacility(int facilityId)
    {
        var inspections = await _inspectionService.GetInspectionsByFacilityAsync(facilityId);
        return Ok(inspections);
    }

    /// <summary>
    /// Retrieves a specific inspection by its identifier.
    /// </summary>
    /// <param name="id">The inspection ID.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var inspection = await _inspectionService.GetInspectionByIdAsync(id);
        if (inspection is null)
            return NotFound(new { message = $"Inspection {id} not found." });

        return Ok(inspection);
    }

    /// <summary>
    /// Schedules a new inspection for a facility.
    /// </summary>
    /// <param name="request">The inspection scheduling request.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Schedule([FromBody] ScheduleInspectionRequest request)
    {
        if (!Enum.TryParse<InspectionPriority>(request.Priority, true, out var priority))
            priority = InspectionPriority.Medium;

        var inspection = new Inspection
        {
            FacilityId = request.FacilityId,
            InspectorName = request.InspectorName,
            InspectionType = request.InspectionType,
            ScheduledDate = request.ScheduledDate,
            Priority = priority,
            Notes = request.Notes
        };

        try
        {
            var created = await _inspectionService.ScheduleInspectionAsync(inspection);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cancels a scheduled inspection.
    /// </summary>
    /// <param name="id">The inspection ID to cancel.</param>
    [HttpDelete("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _inspectionService.CancelInspectionAsync(id);
        if (!result)
            return NotFound(new { message = $"Inspection {id} not found or could not be cancelled." });

        return NoContent();
    }
}
