using Application.Utility;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record ProjectDto(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    ProjectStatusDto ProjectStatus
);

public class CreateProjectDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "ProjectStatusId is required")]
    [GuidNotEmpty(ErrorMessage = "ProjectStatusId must be a valid GUID")]
    public Guid ProjectStatusId { get; set; }

    public CreateProjectDto() { }

    public CreateProjectDto(string name, string? description, Guid projectStatusId)
    {
        Name = name;
        Description = description;
        ProjectStatusId = projectStatusId;
    }
}