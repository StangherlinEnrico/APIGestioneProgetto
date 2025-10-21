using Application.DTOs;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Tests.Application.DTOs;

public class CreateProjectStatusDtoTests
{
    [Fact]
    public void Validate_ShouldFail_WhenNameIsEmpty()
    {
        var dto = new CreateProjectStatusDto("");
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, context, validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameIsTooLong()
    {
        var dto = new CreateProjectStatusDto(new string('a', 51));
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, context, validationResults, true);

        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Validate_ShouldPass_WhenNameIsValid()
    {
        var dto = new CreateProjectStatusDto("Active");
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto);

        var isValid = Validator.TryValidateObject(dto, context, validationResults, true);

        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }
}