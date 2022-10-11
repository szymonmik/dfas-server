using server.Models;

namespace server.Services;

public interface IUserService
{
	void RegisterUser(RegisterUserDto dto);
	string GenerateJwt(LoginDto dto);
}