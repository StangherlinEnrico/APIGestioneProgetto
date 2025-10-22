using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record ProjectStatusDto(Guid Id, string Name, int Priority);

public class CreateProjectStatusDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Priority is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Priority must be at least 1")]
    public int Priority { get; set; }

    public CreateProjectStatusDto() { }

    public CreateProjectStatusDto(string name, int priority)
    {
        Name = name;
        Priority = priority;
    }
}

public class UpdateProjectStatusDto
{
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string? Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Priority must be at least 1")]
    public int? Priority { get; set; }

    public UpdateProjectStatusDto() { }

    public UpdateProjectStatusDto(string? name, int? priority)
    {
        Name = name;
        Priority = priority;
    }
}

