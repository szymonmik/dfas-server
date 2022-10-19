using server.Models;
using System.Security.Claims;

namespace server.Services;

public interface IUserService
{
	void RegisterUser(RegisterUserDto dto);
	string GenerateJwt(LoginDto dto);
	void UpdateUserRegion(int id, UpdateUserRegionDto dto, ClaimsPrincipal userPrincipal);
	void UpdateUserName(int id, UpdateUserNameDto dto, ClaimsPrincipal userPrincipal);
	void UpdateUserSex(int id, UpdateUserSexDto dto, ClaimsPrincipal userPrincipal);
}