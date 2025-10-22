using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectRepository(ApplicationDbContext context) : IProjectRepository
{
    public async Task<Project> AddAsync(Project project)
    {
        await context.Projects.AddAsync(project);
        await context.SaveChangesAsync();
        return project;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await context.Projects
            .Include(p => p.ProjectStatus)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await context.Projects
            .Include(p => p.ProjectStatus)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        context.Projects.Update(project);
        await context.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await context.Projects.FindAsync(id);

        if (project is null)
        {
            return false;
        }

        context.Projects.Remove(project);
        await context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> ExistsByStatusIdAsync(Guid statusId)
    {
        return await context.Projects.AnyAsync(p => p.ProjectStatusId == statusId);
    }
}
