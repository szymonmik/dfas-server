﻿using server.Models;
using System.Security.Claims;
using server.Entities;

namespace server.Services;

public interface IUserService
{
	void RegisterUser(RegisterUserDto dto);
	AuthenticationResponse GenerateJwt(LoginDto dto);
	User GetUser(int id, ClaimsPrincipal userPrincipal);
	void UpdateUserRegion(int id, UpdateUserRegionDto dto, ClaimsPrincipal userPrincipal);
	void UpdateUserName(int id, UpdateUserNameDto dto, ClaimsPrincipal userPrincipal);
	void UpdateUserSex(int id, UpdateUserSexDto dto, ClaimsPrincipal userPrincipal);
	void UpdateUser(int id, UpdateUserDto dto, ClaimsPrincipal userPrincipal);
	public void UpdateUserPassword(int id, UpdateUserPasswordDto dto, ClaimsPrincipal userPrincipal);
}