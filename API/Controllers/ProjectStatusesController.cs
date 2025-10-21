using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectStatusesController : ControllerBase
{
    private readonly IProjectStatusService _projectStatusService;

    public ProjectStatusesController(IProjectStatusService projectStatusService)
    {
        _projectStatusService = projectStatusService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var statuses = await _projectStatusService.GetAllProjectStatusesAsync();
        return Ok(statuses);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var status = await _projectStatusService.GetProjectStatusByIdAsync(id);

        if (status is null)
        {
            return NotFound();
        }

        return Ok(status);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectStatusDto createProjectStatusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var status = await _projectStatusService.CreateProjectStatusAsync(createProjectStatusDto);
        return CreatedAtAction(nameof(GetById), new { id = status.Id }, status);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _projectStatusService.DeleteProjectStatusAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectStatusDto updateProjectStatusDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _projectStatusService.UpdateProjectStatusAsync(id, updateProjectStatusDto);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }
}
