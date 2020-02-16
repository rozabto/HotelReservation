using FluentValidation;

namespace Application.Admin.Commands.CreateEmployee
{
    public class CreateEmployeeValidation : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeValidation()
        {
            RuleFor(f => f.MiddleName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(f => f.UserId)
                .NotEmpty()
                .Length(36);
        }
    }
}
