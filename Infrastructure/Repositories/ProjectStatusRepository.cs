using Domain.Entities;
using Infrastructure.Persistence;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectStatusRepository(ApplicationDbContext context) : IProjectStatusRepository
{
    public async Task<ProjectStatus> AddAsync(ProjectStatus projectStatus)
    {
        await context.ProjectStatuses.AddAsync(projectStatus);
        await context.SaveChangesAsync();
        return projectStatus;
    }

    public async Task<IEnumerable<ProjectStatus>> GetAllAsync()
    {
        return await context.ProjectStatuses
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ProjectStatus?> GetByIdAsync(Guid id)
    {
        return await context.ProjectStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(ps => ps.Id == id);
    }

    public async Task<ProjectStatus> UpdateAsync(ProjectStatus projectStatus)
    {
        context.ProjectStatuses.Update(projectStatus);
        await context.SaveChangesAsync();
        return projectStatus;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var projectStatus = await context.ProjectStatuses.FindAsync(id);

        if (projectStatus is null)
        {
            return false;
        }

        context.ProjectStatuses.Remove(projectStatus);
        await context.SaveChangesAsync();
        return true;
    }
}
