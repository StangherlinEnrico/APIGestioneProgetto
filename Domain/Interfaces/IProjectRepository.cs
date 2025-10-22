using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project> AddAsync(Project project);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task<Project> UpdateAsync(Project project);
    Task<bool> DeleteAsync(Guid id);

    Task<bool> ExistsByStatusIdAsync(Guid statusId);
}
