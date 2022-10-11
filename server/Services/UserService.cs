using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Entities;
using server.Exceptions;
using server.Models;

namespace server.Services;

public class UserService : IUserService
{
	private readonly AppDbContext _context;
	private readonly IPasswordHasher<User> _passwordHasher;
	private readonly AuthenticationSettings _authenticationSettings;
	private readonly IMapper _mapper;

	public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper)
	{
		_context = context;
		_passwordHasher = passwordHasher;
		_authenticationSettings = authenticationSettings;
		_mapper = mapper;
	}
	
	public void RegisterUser(RegisterUserDto dto)
	{
		var newUser = new User()
		{
			Name = dto.Name,
			Email = dto.Email,
			BirthDate = dto.BirthDate,
			RoleId = dto.RoleId,
			VoivodeshipId = dto.VoivodeshipId
		};

		var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
		newUser.PasswordHash = hashedPassword;

		_context.Users.Add(newUser);
		_context.SaveChanges();
	}

	public string GenerateJwt(LoginDto dto)
	{
		var user = _context.Users
			.Include(u => u.Role)
			.FirstOrDefault(u => u.Email == dto.Email);

		if(user is null)
		{
			throw new BadRequestException("Invalid username or password");
		}

		var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
		if(result == PasswordVerificationResult.Failed)
		{
			throw new BadRequestException("Invalid username or password");
		}

		var claims = new List<Claim>()
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, $"{user.Name}"),
			new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
			new Claim("DateOfBirth", user.BirthDate.Value.ToString("yyyy-MM-dd")),
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
		var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

		var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
			_authenticationSettings.JwtIssuer,
			claims,
			expires: expires,
			signingCredentials: cred);

		var tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.WriteToken(token);
	}
}