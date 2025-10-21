using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(Guid id);
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
    Task<bool> DeleteProjectAsync(Guid id);
    Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto);
}
