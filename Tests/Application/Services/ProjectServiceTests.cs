using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Application.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _service = new ProjectService(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnAllProjects()
    {
        var status = ProjectStatus.Create("Active");
        var project1 = Project.Create("Project 1", "Description 1", status.Id);
        var project2 = Project.Create("Project 2", "Description 2", status.Id);

        typeof(Project).GetProperty("ProjectStatus")!.SetValue(project1, status);
        typeof(Project).GetProperty("ProjectStatus")!.SetValue(project2, status);

        _projectRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new[] { project1, project2 });

        var result = await _service.GetAllProjectsAsync();

        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Project 1");
    }

    [Fact]
    public async Task GetProjectByIdAsync_ShouldReturnProject_WhenExists()
    {
        var status = ProjectStatus.Create("Active");
        var project = Project.Create("Test Project", "Description", status.Id);
        typeof(Project).GetProperty("ProjectStatus")!.SetValue(project, status);

        _projectRepositoryMock.Setup(r => r.GetByIdAsync(project.Id))
            .ReturnsAsync(project);

        var result = await _service.GetProjectByIdAsync(project.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Project");
    }

    [Fact]
    public async Task GetProjectByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Project?)null);

        var result = await _service.GetProjectByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateProjectAsync_ShouldCreateProject()
    {
        var statusId = Guid.NewGuid();
        var status = ProjectStatus.Create("Active");
        var createDto = new CreateProjectDto("New Project", "Description", statusId);

        _projectRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Project>()))
            .ReturnsAsync((Project p) => p);

        _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) =>
            {
                var project = Project.Create("New Project", "Description", statusId);
                typeof(Project).GetProperty("Id")!.SetValue(project, id);
                typeof(Project).GetProperty("ProjectStatus")!.SetValue(project, status);
                return project;
            });

        var result = await _service.CreateProjectAsync(createDto);

        result.Should().NotBeNull();
        result.Name.Should().Be("New Project");
        _projectRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProjectAsync_ShouldReturnTrue_WhenDeleted()
    {
        var projectId = Guid.NewGuid();
        _projectRepositoryMock.Setup(r => r.DeleteAsync(projectId))
            .ReturnsAsync(true);

        var result = await _service.DeleteProjectAsync(projectId);

        result.Should().BeTrue();
        _projectRepositoryMock.Verify(r => r.DeleteAsync(projectId), Times.Once);
    }

    [Fact]
    public async Task DeleteProjectAsync_ShouldReturnFalse_WhenNotFound()
    {
        var projectId = Guid.NewGuid();
        _projectRepositoryMock.Setup(r => r.DeleteAsync(projectId))
            .ReturnsAsync(false);

        var result = await _service.DeleteProjectAsync(projectId);

        result.Should().BeFalse();
    }
}
