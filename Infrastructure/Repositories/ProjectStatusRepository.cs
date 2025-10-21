using Domain.Entities;
using Infrastructure.Persistence;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectStatusRepository : IProjectStatusRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectStatusRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectStatus>> GetAllAsync()
    {
        return await _context.ProjectStatuses
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ProjectStatus?> GetByIdAsync(Guid id)
    {
        return await _context.ProjectStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(ps => ps.Id == id);
    }

    public async Task<ProjectStatus?> GetByNameAsync(string name)
    {
        return await _context.ProjectStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(ps => ps.Name == name);
    }

    public async Task<ProjectStatus> AddAsync(ProjectStatus projectStatus)
    {
        await _context.ProjectStatuses.AddAsync(projectStatus);
        await _context.SaveChangesAsync();
        return projectStatus;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var projectStatus = await _context.ProjectStatuses.FindAsync(id);

        if (projectStatus is null)
        {
            return false;
        }

        _context.ProjectStatuses.Remove(projectStatus);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ProjectStatus> UpdateAsync(ProjectStatus projectStatus)
    {
        _context.ProjectStatuses.Update(projectStatus);
        await _context.SaveChangesAsync();
        return projectStatus;
    }
}
