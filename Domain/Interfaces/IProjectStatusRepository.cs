using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectStatusRepository
{
    Task<IEnumerable<ProjectStatus>> GetAllAsync();
    Task<ProjectStatus?> GetByIdAsync(Guid id);
    Task<ProjectStatus?> GetByNameAsync(string name);
    Task<ProjectStatus> AddAsync(ProjectStatus projectStatus);
    Task<bool> DeleteAsync(Guid id);
    Task<ProjectStatus> UpdateAsync(ProjectStatus projectStatus);
}
