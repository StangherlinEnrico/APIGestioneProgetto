using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Infrastructure.Repositories;

public class ProjectRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ProjectRepository _repository;

    public ProjectRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new ProjectRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProjects()
    {
        var status = ProjectStatus.Create("Active");
        await _context.ProjectStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        var project1 = Project.Create("Project 1", "Description 1", status.Id);
        var project2 = Project.Create("Project 2", "Description 2", status.Id);
        await _context.Projects.AddRangeAsync(project1, project2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProject_WhenExists()
    {
        var status = ProjectStatus.Create("Active");
        await _context.ProjectStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        var project = Project.Create("Test Project", "Description", status.Id);
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(project.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(project.Id);
        result.Name.Should().Be("Test Project");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddProject()
    {
        var status = ProjectStatus.Create("Active");
        await _context.ProjectStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        var project = Project.Create("New Project", "Description", status.Id);

        var result = await _repository.AddAsync(project);

        result.Should().NotBeNull();
        result.Id.Should().Be(project.Id);

        var savedProject = await _context.Projects.FindAsync(project.Id);
        savedProject.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProject_WhenExists()
    {
        var status = ProjectStatus.Create("Active");
        await _context.ProjectStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        var project = Project.Create("Test Project", "Description", status.Id);
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        var result = await _repository.DeleteAsync(project.Id);

        result.Should().BeTrue();

        var deletedProject = await _context.Projects.FindAsync(project.Id);
        deletedProject.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
    {
        var result = await _repository.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByStatusIdAsync_ShouldReturnTrue_WhenProjectsExist()
    {
        var status = ProjectStatus.Create("Active");
        await _context.ProjectStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        var project = Project.Create("Test Project", "Description", status.Id);
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        var result = await _repository.ExistsByStatusIdAsync(status.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByStatusIdAsync_ShouldReturnFalse_WhenNoProjectsExist()
    {
        var result = await _repository.ExistsByStatusIdAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
