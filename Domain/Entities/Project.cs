namespace Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public Guid ProjectStatusId { get; private set; }
    public ProjectStatus ProjectStatus { get; private set; } = null!;

    private Project() { }

    public static Project Create(string name, string description, Guid projectStatusId)
    {
        return new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            ProjectStatusId = projectStatusId
        };
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        Name = name;
    }

    public void UpdateDescription(string description)
    {
        Description = description ?? string.Empty;
    }

    public void UpdateStatus(Guid projectStatusId)
    {
        if (projectStatusId == Guid.Empty)
            throw new ArgumentException("ProjectStatusId cannot be empty", nameof(projectStatusId));
        ProjectStatusId = projectStatusId;
    }
}
