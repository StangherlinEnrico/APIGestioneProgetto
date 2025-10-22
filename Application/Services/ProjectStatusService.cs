using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ProjectStatusService(
    IProjectStatusRepository projectStatusRepository,
    IProjectRepository projectRepository) : IProjectStatusService
{
    public async Task<ProjectStatusDto> CreateProjectStatusAsync(CreateProjectStatusDto createProjectStatusDto)
    {
        var allStatuses = (await projectStatusRepository.GetAllAsync()).ToList();

        var statusesAtOrAfter = allStatuses
            .Where(s => s.Priority >= createProjectStatusDto.Priority)
            .OrderBy(s => s.Priority)
            .ToList();

        foreach (var status in statusesAtOrAfter)
        {
            status.UpdatePriority(status.Priority + 1);
            await projectStatusRepository.UpdateAsync(status);
        }

        var projectStatus = ProjectStatus.Create(createProjectStatusDto.Name, createProjectStatusDto.Priority);
        var createdStatus = await projectStatusRepository.AddAsync(projectStatus);
        return new ProjectStatusDto(createdStatus.Id, createdStatus.Name, createdStatus.Priority);
    }

    public async Task<IEnumerable<ProjectStatusDto>> GetAllProjectStatusesAsync()
    {
        var statuses = await projectStatusRepository.GetAllAsync();
        return statuses.Select(s => new ProjectStatusDto(s.Id, s.Name, s.Priority));
    }

    public async Task<ProjectStatusDto?> GetProjectStatusByIdAsync(Guid id)
    {
        var status = await projectStatusRepository.GetByIdAsync(id);
        return status is null ? null : new ProjectStatusDto(status.Id, status.Name, status.Priority);
    }

    public async Task<ProjectStatusDto?> UpdateProjectStatusAsync(Guid id, UpdateProjectStatusDto updateProjectStatusDto)
    {
        var status = await projectStatusRepository.GetByIdAsync(id);
        if (status is null) return null;

        if (!string.IsNullOrWhiteSpace(updateProjectStatusDto.Name))
            status.UpdateName(updateProjectStatusDto.Name);

        if (updateProjectStatusDto.Priority.HasValue)
        {
            var oldPriority = status.Priority;
            var newPriority = updateProjectStatusDto.Priority.Value;

            if (oldPriority != newPriority)
            {
                var allStatuses = (await projectStatusRepository.GetAllAsync()).ToList();

                if (newPriority < oldPriority)
                {
                    var statusesToShift = allStatuses
                        .Where(s => s.Id != id && s.Priority >= newPriority && s.Priority < oldPriority)
                        .OrderBy(s => s.Priority)
                        .ToList();

                    foreach (var statusToShift in statusesToShift)
                    {
                        statusToShift.UpdatePriority(statusToShift.Priority + 1);
                        await projectStatusRepository.UpdateAsync(statusToShift);
                    }
                }
                else
                {
                    var statusesToShift = allStatuses
                        .Where(s => s.Id != id && s.Priority > oldPriority && s.Priority <= newPriority)
                        .OrderByDescending(s => s.Priority)
                        .ToList();

                    foreach (var statusToShift in statusesToShift)
                    {
                        statusToShift.UpdatePriority(statusToShift.Priority - 1);
                        await projectStatusRepository.UpdateAsync(statusToShift);
                    }
                }

                status.UpdatePriority(newPriority);
            }
        }

        var updated = await projectStatusRepository.UpdateAsync(status);
        return new ProjectStatusDto(updated.Id, updated.Name, updated.Priority);
    }

    public async Task<bool> DeleteProjectStatusAsync(Guid id)
    {
        var hasProjects = await projectRepository.ExistsByStatusIdAsync(id);
        if (hasProjects)
        {
            throw new InvalidOperationException($"Cannot delete ProjectStatus with Id {id} because it is associated with one or more projects");
        }

        var statusToDelete = await projectStatusRepository.GetByIdAsync(id);
        if (statusToDelete is null) return false;

        var deletedPriority = statusToDelete.Priority;
        var deleted = await projectStatusRepository.DeleteAsync(id);

        if (deleted)
        {
            var allStatuses = (await projectStatusRepository.GetAllAsync()).ToList();
            var statusesAfter = allStatuses
                .Where(s => s.Priority > deletedPriority)
                .OrderBy(s => s.Priority)
                .ToList();

            foreach (var status in statusesAfter)
            {
                status.UpdatePriority(status.Priority - 1);
                await projectStatusRepository.UpdateAsync(status);
            }
        }

        return deleted;
    }
}