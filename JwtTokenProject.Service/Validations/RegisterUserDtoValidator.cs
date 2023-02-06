using FluentValidation;
using JwtTokenProject.Common.DTOs;

namespace JwtTokenProject.Service.Validations
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(i => i.Email).NotNull().WithMessage($"This {0} required").EmailAddress().WithMessage($"This {0} wrong");
        }

    }
}
