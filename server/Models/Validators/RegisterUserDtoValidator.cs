using FluentValidation;
using server.Entities;

namespace server.Models.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
	public RegisterUserDtoValidator(AppDbContext dbContext)
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();

		RuleFor(x => x.Password)
			.MinimumLength(8);

		RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
		
		RuleFor(x => x.BirthDate).LessThan(e => DateTime.Now);

		RuleFor(x => x.Email)
			.Custom((value, context) =>
			{
				var emailInUse = dbContext.Users.Any(u => u.Email == value);
				if (emailInUse)
				{
					context.AddFailure("Email", "That email is taken");
				}
			});
	}
}