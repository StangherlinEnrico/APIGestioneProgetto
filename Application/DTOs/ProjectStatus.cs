using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record ProjectStatusDto(
    Guid Id,
    string Name
);

public class CreateProjectStatusDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    public CreateProjectStatusDto() { }

    public CreateProjectStatusDto(string name)
    {
        Name = name;
    }
}

public class UpdateProjectStatusDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    public UpdateProjectStatusDto() { }

    public UpdateProjectStatusDto(string name)
    {
        Name = name;
    }
}
