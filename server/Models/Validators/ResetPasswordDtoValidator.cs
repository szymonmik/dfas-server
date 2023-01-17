using FluentValidation;
using server.Entities;

namespace server.Models.Validators;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
	public ResetPasswordDtoValidator(AppDbContext dbContext)
	{
		RuleFor(x => x.Password)
			.MinimumLength(8)
			.WithMessage("Hasło musi mieć co najmniej 8 znaków");

		RuleFor(x => x.ConfirmPassword).Equal(e => e.Password).WithMessage("Hasła nie są zgodne.");
	}
}