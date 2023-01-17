using FluentValidation;
using server.Entities;

namespace server.Models.Validators;

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
	public ForgotPasswordDtoValidator(AppDbContext dbContext)
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();
		
		RuleFor(x => x.Email)
			.Custom((value, context) =>
			{
				var emailInUse = dbContext.Users.Any(u => u.Email == value);
				if (!emailInUse)
				{
					context.AddFailure("Email", "Nie znaleziono adresu email.");
				}
			});
	}
}