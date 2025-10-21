using Domain.Entities;
using FluentAssertions;

namespace Tests.Domain.Entities;

public class ProjectTests
{
    [Fact]
    public void Create_ShouldCreateProjectWithValidData()
    {
        var name = "Test Project";
        var description = "Test Description";
        var statusId = Guid.NewGuid();

        var project = Project.Create(name, description, statusId);

        project.Should().NotBeNull();
        project.Id.Should().NotBeEmpty();
        project.Name.Should().Be(name);
        project.Description.Should().Be(description);
        project.ProjectStatusId.Should().Be(statusId);
        project.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_ShouldGenerateUniqueIds()
    {
        var statusId = Guid.NewGuid();

        var project1 = Project.Create("Project 1", "Description 1", statusId);
        var project2 = Project.Create("Project 2", "Description 2", statusId);

        project1.Id.Should().NotBe(project2.Id);
    }

    [Fact]
    public void Create_ShouldAcceptEmptyDescription()
    {
        var statusId = Guid.NewGuid();

        var project = Project.Create("Test Project", string.Empty, statusId);

        project.Description.Should().BeEmpty();
    }
}
