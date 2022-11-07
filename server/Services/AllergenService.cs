using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using server.Authorization;
using server.Entities;
using server.Exceptions;
using server.Models;

namespace server.Services;

public class AllergenService : IAllergenService
{
	private readonly AppDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IAuthorizationService _authorizationService;
	
	public AllergenService(AppDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_authorizationService = authorizationService;
	}
	
	public IEnumerable<AllergenDto> GetAllAllergens()
	{
		var allergens = _dbContext.Allergens
			.Include(a => a.AllergenType)
			.ToList();

		var allergenDtos = _mapper.Map<List<AllergenDto>>(allergens);

		return allergenDtos;
	}

	public AllergenDto GetAllergenById(int allergenId)
	{
		var allergen = _dbContext.Allergens
			.Include(a => a.AllergenType)
			.FirstOrDefault(a => a.Id == allergenId);

		if (allergen is null)
		{
			throw new NotFoundException("Allergen not found");
		}

		var allergenDto = _mapper.Map<AllergenDto>(allergen);

		return allergenDto;
	}
	
	public IEnumerable<AllergenType> GetAllAllergenTypes()
	{
		var allergenTypes = _dbContext.AllergenTypes
			.ToList();
		
		return allergenTypes;
	}
	
	public AllergenType GetAllergenTypeById(int allergenTypeId)
	{
		var allergenType = _dbContext.AllergenTypes
			.FirstOrDefault(a => a.Id == allergenTypeId);

		if (allergenType is null)
		{
			throw new NotFoundException("Allergen not found");
		}

		return allergenType;
	}
}