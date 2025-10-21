using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IProjectStatusService _projectStatusService;

    public ProjectsController(IProjectService projectService, IProjectStatusService projectStatusService)
    {
        _projectService = projectService;
        _projectStatusService = projectStatusService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project is null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var statusExists = await _projectStatusService.GetProjectStatusByIdAsync(createProjectDto.ProjectStatusId);
        if (statusExists is null)
        {
            ModelState.AddModelError("ProjectStatusId", $"ProjectStatus with Id {createProjectDto.ProjectStatusId} does not exist");
            return BadRequest(ModelState);
        }

        var project = await _projectService.CreateProjectAsync(createProjectDto);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _projectService.DeleteProjectAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
