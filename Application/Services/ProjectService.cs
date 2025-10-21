using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects.Select(p => new ProjectDto(
            p.Id,
            p.Name,
            p.Description,
            p.CreatedAt,
            new ProjectStatusDto(p.ProjectStatus.Id, p.ProjectStatus.Name)
        ));
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project is null
            ? null
            : new ProjectDto(
                project.Id,
                project.Name,
                project.Description,
                project.CreatedAt,
                new ProjectStatusDto(project.ProjectStatus.Id, project.ProjectStatus.Name)
            );
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
    {
        var project = Project.Create(
            createProjectDto.Name,
            createProjectDto.Description ?? string.Empty,
            createProjectDto.ProjectStatusId
        );

        var createdProject = await _projectRepository.AddAsync(project);
        var projectWithStatus = await _projectRepository.GetByIdAsync(createdProject.Id);

        return new ProjectDto(
            projectWithStatus!.Id,
            projectWithStatus.Name,
            projectWithStatus.Description,
            projectWithStatus.CreatedAt,
            new ProjectStatusDto(projectWithStatus.ProjectStatus.Id, projectWithStatus.ProjectStatus.Name)
        );
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        return await _projectRepository.DeleteAsync(id);
    }

    public async Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project is null) return null;

        if (!string.IsNullOrWhiteSpace(updateProjectDto.Name))
            project.UpdateName(updateProjectDto.Name);

        if (updateProjectDto.Description is not null)
            project.UpdateDescription(updateProjectDto.Description);

        if (updateProjectDto.ProjectStatusId.HasValue)
            project.UpdateStatus(updateProjectDto.ProjectStatusId.Value);

        var updated = await _projectRepository.UpdateAsync(project);

        var refreshed = await _projectRepository.GetByIdAsync(updated.Id);
        return new ProjectDto(
            refreshed!.Id,
            refreshed.Name,
            refreshed.Description,
            refreshed.CreatedAt,
            new ProjectStatusDto(refreshed.ProjectStatus.Id, refreshed.ProjectStatus.Name)
        );
    }
}
