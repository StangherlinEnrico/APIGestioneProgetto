using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto);
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(Guid id);
    Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectDto updateProjectDto);
    Task<bool> DeleteProjectAsync(Guid id);
}
