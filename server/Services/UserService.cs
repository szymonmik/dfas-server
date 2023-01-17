using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Authorization;
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
	private readonly IAuthorizationService _authorizationService;

	public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper, IAuthorizationService authorizationService)
	{
		_context = context;
		_passwordHasher = passwordHasher;
		_authenticationSettings = authenticationSettings;
		_mapper = mapper;
		_authorizationService = authorizationService;
	}
	
	public void RegisterUser(RegisterUserDto dto)
	{
		var newUser = new User()
		{
			Name = dto.Name,
			Email = dto.Email,
			BirthDate = dto.BirthDate,
			Sex = dto.Sex,
			RoleId = dto.RoleId,
			RegionId = dto.RegionId
		};

		var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
		newUser.PasswordHash = hashedPassword;

		_context.Users.Add(newUser);
		_context.SaveChanges();
	}

	public AuthenticationResponse GenerateJwt(LoginDto dto)
	{
		var user = _context.Users
			.Include(u => u.Role)
			.Include(u => u.Region)
			.Include(x => x.UserAllergens)
			.ThenInclude(x => x.Allergen)
			.ThenInclude(x => x.AllergenType)
			.FirstOrDefault(u => u.Email == dto.Email);

		if(user is null)
		{
			throw new BadRequestException("Nieprawidłowy email lub hasło");
		}

		var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
		if(result == PasswordVerificationResult.Failed)
		{
			throw new BadRequestException("Nieprawidłowy email lub hasło");
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
		var tokenString = tokenHandler.WriteToken(token);
		
		var userDto = _mapper.Map<UserDto>(user);
		var authenticationResponse = new AuthenticationResponse(userDto, tokenString);
		return authenticationResponse;
	}

	public UserDto GetUser(int id, ClaimsPrincipal userPrincipal)
    {
		var user = _context.Users
			.Include(u => u.Role)
			.Include(u => u.Region)
			.Include(x => x.UserAllergens)
			.ThenInclude(x => x.Allergen)
			.FirstOrDefault(u => u.Id == id);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}
		
		var userDto = _mapper.Map<UserDto>(user);

		return userDto;
	}

	public void UpdateUserRegion(int id, UpdateUserRegionDto dto, ClaimsPrincipal userPrincipal)
	{
		var user = _context.Users.FirstOrDefault(u => u.Id == id);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		user.RegionId = dto.RegionId;

		_context.SaveChanges();
	}

	public void UpdateUserName(int id, UpdateUserNameDto dto, ClaimsPrincipal userPrincipal)
	{
		var user = _context.Users.FirstOrDefault(u => u.Id == id);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		user.Name = dto.Name;

		_context.SaveChanges();
	}

	public void UpdateUserSex(int id, UpdateUserSexDto dto, ClaimsPrincipal userPrincipal)
	{
		var user = _context.Users.FirstOrDefault(u => u.Id == id);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		if (dto.Sex == 'k' || dto.Sex == 'm') user.Sex = dto.Sex;

		_context.SaveChanges();
	}

	public void UpdateUser(int id, UpdateUserDto dto, ClaimsPrincipal userPrincipal)
	{
		var user = _context.Users.FirstOrDefault(u => u.Id == id);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, user, 
			new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		if(dto.Sex == 'k' || dto.Sex == 'm')user.Sex = dto.Sex;
		if(!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
		if(dto.RegionId >= 1 && dto.RegionId <= 4) user.RegionId = dto.RegionId;
		
		_context.SaveChanges();
	}
	
	public void UpdateUserPassword(int id, UpdateUserPasswordDto dto, ClaimsPrincipal userPrincipal)
	{
		var user = _context.Users.FirstOrDefault(u => u.Id == id);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}
		
		var newHashedPassword = _passwordHasher.HashPassword(user, dto.Password);
		user.PasswordHash = newHashedPassword;

		_context.SaveChanges();
	}
	
	public void AssignAllergen(int allergenId, ClaimsPrincipal userPrincipal)
	{
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _context.Users.FirstOrDefault(u => u.Id == userId);

		var allergen = _context.Allergens
			.Include(x => x.AllergenType)
			.FirstOrDefault(x => x.Id == allergenId);

		if (allergen is null)
		{
			throw new NotFoundException("Nie znaleziono alergenu");
		}

		if (allergen.AllergenType.Name != "Wziewny")
		{
			throw new BadRequestException("Zły typ alergenu");
		}
		
		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}
		
		var existingAssignment = _context.UserHasAllergens.FirstOrDefault(x => x.UserId == userId && x.AllergenId == allergenId);

		if (existingAssignment != null)
		{
			throw new BadRequestException("Już przypisano");
		}

		var assignment = new UserHasAllergen
		{
			UserId = userId,
			AllergenId = allergenId
		};
		
		_context.UserHasAllergens.Add(assignment);
		_context.SaveChanges();
	}

	public void UnassignAllergen(int allergenId, ClaimsPrincipal userPrincipal)
	{
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _context.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		var existingAssignment = _context.UserHasAllergens.FirstOrDefault(x => x.UserId == userId && x.AllergenId == allergenId);

		if (existingAssignment is null)
		{
			throw new BadRequestException("Przypisanie nie istnieje");
		}

		_context.Remove(existingAssignment);
		_context.SaveChanges();
	}

	public string GeneratePasswordResetToken(ForgotPasswordDto dto)
	{
		var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);
		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var existingToken = _context.PasswordResetTokens.FirstOrDefault(x => x.UserId == user.Id);

		if (existingToken != null)
		{
			return existingToken.Token;
		}
		
		var token = Guid.NewGuid().ToString();
		var passwordResetToken = new PasswordResetToken()
		{
			UserId = user.Id,
			Token = token
		};
		_context.Add(passwordResetToken);
		_context.SaveChanges();

		return token;
	}
	
	public void ResetPassword(ResetPasswordDto dto)
	{
		var user = _context.Users.FirstOrDefault(x => x.Id == dto.UserId);
		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var existingToken = _context.PasswordResetTokens.FirstOrDefault(x => x.UserId == dto.UserId && x.Token == dto.Token);

		if (existingToken is null)
		{
			throw new BadRequestException("Nie udało się zweryfikować tokenu");
		}
		
		var newHashedPassword = _passwordHasher.HashPassword(user, dto.Password);
		user.PasswordHash = newHashedPassword;
		
		_context.Remove(existingToken);
		_context.SaveChanges();
	}
}