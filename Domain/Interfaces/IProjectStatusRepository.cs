using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectStatusRepository
{
    Task<ProjectStatus> AddAsync(ProjectStatus projectStatus);
    Task<IEnumerable<ProjectStatus>> GetAllAsync();
    Task<ProjectStatus?> GetByIdAsync(Guid id);
    Task<ProjectStatus> UpdateAsync(ProjectStatus projectStatus);
    Task<bool> DeleteAsync(Guid id);
}
