namespace Domain.Entities;

public class ProjectStatus
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private ProjectStatus() { }

    public static ProjectStatus Create(string name)
    {
        return new ProjectStatus
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        Name = name;
    }
}
