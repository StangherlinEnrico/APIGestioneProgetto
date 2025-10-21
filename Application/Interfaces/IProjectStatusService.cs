using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectStatusService
{
    Task<IEnumerable<ProjectStatusDto>> GetAllProjectStatusesAsync();
    Task<ProjectStatusDto?> GetProjectStatusByIdAsync(Guid id);
    Task<ProjectStatusDto> CreateProjectStatusAsync(CreateProjectStatusDto createProjectStatusDto);
    Task<bool> DeleteProjectStatusAsync(Guid id);
    Task<ProjectStatusDto?> UpdateProjectStatusAsync(Guid id, UpdateProjectStatusDto updateProjectStatusDto);
}
