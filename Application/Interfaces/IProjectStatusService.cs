using Application.DTOs;

namespace Application.Interfaces;

public interface IProjectStatusService
{
    Task<ProjectStatusDto> CreateProjectStatusAsync(CreateProjectStatusDto createProjectStatusDto);
    Task<IEnumerable<ProjectStatusDto>> GetAllProjectStatusesAsync();
    Task<ProjectStatusDto?> GetProjectStatusByIdAsync(Guid id);
    Task<ProjectStatusDto?> UpdateProjectStatusAsync(Guid id, UpdateProjectStatusDto updateProjectStatusDto);
    Task<bool> DeleteProjectStatusAsync(Guid id);
}
