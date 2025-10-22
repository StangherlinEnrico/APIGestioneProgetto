namespace Domain.Entities;

public class ProjectStatus
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Priority { get; private set; }

    private ProjectStatus() { }

    public static ProjectStatus Create(string name, int priority)
    {
        if (priority < 1)
            throw new ArgumentException("Priority must be at least 1", nameof(priority));

        return new ProjectStatus
        {
            Id = Guid.NewGuid(),
            Name = name,
            Priority = priority
        };
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        Name = name;
    }

    public void UpdatePriority(int priority)
    {
        if (priority < 1)
            throw new ArgumentException("Priority must be at least 1", nameof(priority));
        Priority = priority;
    }
}