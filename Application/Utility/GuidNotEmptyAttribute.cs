using System.ComponentModel.DataAnnotations;

namespace Application.Utility;

public class GuidNotEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is Guid guid && guid == Guid.Empty)
        {
            return new ValidationResult(ErrorMessage ?? "GUID cannot be empty", [validationContext.MemberName ?? "ProjectStatusId"]);
        }
        return ValidationResult.Success;
    }
}