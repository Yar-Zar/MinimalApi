using FluentValidation;
using System.Data;
using TestMinimalApi.Application.DTOs;

namespace TestMinimalApi.Application.Validators
{
    public class RegionValidator : AbstractValidator<Region>
    {
        public RegionValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Region Code is required.")
                .MaximumLength(10).WithMessage("Region Code must not exceed 10 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Region Name is required.")
                .MaximumLength(100).WithMessage("Region Name must not exceed 100 characters.")
                .Must(name => !string.IsNullOrEmpty(name) && char.IsUpper(name[0]))
                .WithMessage("Region Name must start with an uppercase letter.")
                .When(x => !string.IsNullOrEmpty(x.Name)); 
        }
    }
}
