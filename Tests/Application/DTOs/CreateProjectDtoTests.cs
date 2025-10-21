using Application.DTOs;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Tests.Application.DTOs;

public class CreateProjectDtoTests
{
    [Fact]
    public void Validate_ShouldFail_WhenNameIsEmpty()
    {
        var dto = new CreateProjectDto("", null, Guid.NewGuid());
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, context, validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameIsTooLong()
    {
        var dto = new CreateProjectDto(new string('a', 201), null, Guid.NewGuid());
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, context, validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Validate_ShouldFail_WhenProjectStatusIdIsEmpty()
    {
        var dto = new CreateProjectDto("Valid Name", null, Guid.Empty);
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, context, validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("ProjectStatusId"));
    }

    [Fact]
    public void Validate_ShouldPass_WhenAllFieldsValid()
    {
        var dto = new CreateProjectDto("Valid Name", "Valid Description", Guid.NewGuid());
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, context, validationResults, true);

        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }
}