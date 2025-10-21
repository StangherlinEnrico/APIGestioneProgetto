using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task<Project> AddAsync(Project project);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByStatusIdAsync(Guid statusId);
}
