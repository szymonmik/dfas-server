using server.Models;

namespace server.Services;

public interface IMailService
{
	Task SendPasswordReset(ForgotPasswordDto dto, string token);
}