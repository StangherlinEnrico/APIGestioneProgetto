using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Application.Services;

public class ProjectStatusServiceTests
{
    private readonly Mock<IProjectStatusRepository> _statusRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly ProjectStatusService _service;

    public ProjectStatusServiceTests()
    {
        _statusRepositoryMock = new Mock<IProjectStatusRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _service = new ProjectStatusService(_statusRepositoryMock.Object, _projectRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllProjectStatusesAsync_ShouldReturnAllStatuses()
    {
        var status1 = ProjectStatus.Create("Active");
        var status2 = ProjectStatus.Create("Completed");

        _statusRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new[] { status1, status2 });

        var result = await _service.GetAllProjectStatusesAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetProjectStatusByIdAsync_ShouldReturnStatus_WhenExists()
    {
        var status = ProjectStatus.Create("Active");

        _statusRepositoryMock.Setup(r => r.GetByIdAsync(status.Id))
            .ReturnsAsync(status);

        var result = await _service.GetProjectStatusByIdAsync(status.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Active");
    }

    [Fact]
    public async Task GetProjectStatusByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _statusRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((ProjectStatus?)null);

        var result = await _service.GetProjectStatusByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateProjectStatusAsync_ShouldCreateStatus()
    {
        var createDto = new CreateProjectStatusDto("New Status");

        _statusRepositoryMock.Setup(r => r.AddAsync(It.IsAny<ProjectStatus>()))
            .ReturnsAsync((ProjectStatus s) => s);

        var result = await _service.CreateProjectStatusAsync(createDto);

        result.Should().NotBeNull();
        result.Name.Should().Be("New Status");
        _statusRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ProjectStatus>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProjectStatusAsync_ShouldDeleteStatus_WhenNoProjectsAssociated()
    {
        var statusId = Guid.NewGuid();

        _projectRepositoryMock.Setup(r => r.ExistsByStatusIdAsync(statusId))
            .ReturnsAsync(false);

        _statusRepositoryMock.Setup(r => r.DeleteAsync(statusId))
            .ReturnsAsync(true);

        var result = await _service.DeleteProjectStatusAsync(statusId);

        result.Should().BeTrue();
        _statusRepositoryMock.Verify(r => r.DeleteAsync(statusId), Times.Once);
    }

    [Fact]
    public async Task DeleteProjectStatusAsync_ShouldThrowException_WhenProjectsAssociated()
    {
        var statusId = Guid.NewGuid();

        _projectRepositoryMock.Setup(r => r.ExistsByStatusIdAsync(statusId))
            .ReturnsAsync(true);

        var act = async () => await _service.DeleteProjectStatusAsync(statusId);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Cannot delete ProjectStatus with Id {statusId} because it is associated with one or more projects");

        _statusRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }
}
