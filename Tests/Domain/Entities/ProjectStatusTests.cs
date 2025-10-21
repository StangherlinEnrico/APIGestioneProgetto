using Domain.Entities;
using FluentAssertions;

namespace Tests.Domain.Entities;

public class ProjectStatusTests
{
    [Fact]
    public void Create_ShouldCreateProjectStatusWithValidData()
    {
        var name = "Active";

        var status = ProjectStatus.Create(name);

        status.Should().NotBeNull();
        status.Id.Should().NotBeEmpty();
        status.Name.Should().Be(name);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueIds()
    {
        var status1 = ProjectStatus.Create("Active");
        var status2 = ProjectStatus.Create("Completed");

        status1.Id.Should().NotBe(status2.Id);
    }
}