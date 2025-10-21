using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ProjectStatusService : IProjectStatusService
{
    private readonly IProjectStatusRepository _projectStatusRepository;
    private readonly IProjectRepository _projectRepository;

    public ProjectStatusService(IProjectStatusRepository projectStatusRepository, IProjectRepository projectRepository)
    {
        _projectStatusRepository = projectStatusRepository;
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectStatusDto>> GetAllProjectStatusesAsync()
    {
        var statuses = await _projectStatusRepository.GetAllAsync();
        return statuses.Select(s => new ProjectStatusDto(s.Id, s.Name));
    }

    public async Task<ProjectStatusDto?> GetProjectStatusByIdAsync(Guid id)
    {
        var status = await _projectStatusRepository.GetByIdAsync(id);
        return status is null ? null : new ProjectStatusDto(status.Id, status.Name);
    }

    public async Task<ProjectStatusDto> CreateProjectStatusAsync(CreateProjectStatusDto createProjectStatusDto)
    {
        var projectStatus = ProjectStatus.Create(createProjectStatusDto.Name);
        var createdStatus = await _projectStatusRepository.AddAsync(projectStatus);
        return new ProjectStatusDto(createdStatus.Id, createdStatus.Name);
    }

    public async Task<bool> DeleteProjectStatusAsync(Guid id)
    {
        var hasProjects = await _projectRepository.ExistsByStatusIdAsync(id);
        if (hasProjects)
        {
            throw new InvalidOperationException($"Cannot delete ProjectStatus with Id {id} because it is associated with one or more projects");
        }

        return await _projectStatusRepository.DeleteAsync(id);
    }
}
