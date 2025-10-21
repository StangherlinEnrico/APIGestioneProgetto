using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Infrastructure.Repositories;

public class ProjectStatusRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ProjectStatusRepository _repository;

    public ProjectStatusRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new ProjectStatusRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStatuses()
    {
        var status1 = ProjectStatus.Create("Active");
        var status2 = ProjectStatus.Create("Completed");
        await _context.ProjectStatuses.AddRangeAsync(status1, status2);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnStatus_WhenExists()
    {
        var status = ProjectStatus.Create("Active");
        await _context.ProjectStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(status.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(status.Id);
        result.Name.Should().Be("Active");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnStatus_WhenExists()
    {
        var status = ProjectStatus.Create("Active");
        await _context.ProjectStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByNameAsync("Active");

        result.Should().NotBeNull();
        result!.Name.Should().Be("Active");
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenNotExists()
    {
        var result = await _repository.GetByNameAsync("NonExistent");

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddStatus()
    {
        var status = ProjectStatus.Create("New Status");

        var result = await _repository.AddAsync(status);

        result.Should().NotBeNull();
        result.Id.Should().Be(status.Id);

        var savedStatus = await _context.ProjectStatuses.FindAsync(status.Id);
        savedStatus.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteStatus_WhenExists()
    {
        var status = ProjectStatus.Create("Test Status");
        await _context.ProjectStatuses.AddAsync(status);
        await _context.SaveChangesAsync();

        var result = await _repository.DeleteAsync(status.Id);

        result.Should().BeTrue();

        var deletedStatus = await _context.ProjectStatuses.FindAsync(status.Id);
        deletedStatus.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
    {
        var result = await _repository.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
