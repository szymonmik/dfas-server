using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Exceptions;
using server.Models;

namespace server.Services;

public class PollinationService : IPollinationService
{
	private readonly AppDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IAuthorizationService _authorizationService;

	public PollinationService(AppDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_authorizationService = authorizationService;
	}
	public IEnumerable<PollinationCalendar> GetByDate(int regionId, string date, ClaimsPrincipal userPrincipal)
	{
		var parsedDate = DateTime.Parse(date);
		var pollinationCalendar = _dbContext.PollinationCalendars
			.Include(x => x.Region)
			.Include(x => x.Allergen)
			.ThenInclude(x => x.AllergenType)
			.Where(x => x.RegionId == regionId && x.Date == parsedDate)
			.ToList();

		return pollinationCalendar;
	}

	public void FillRandomOnDate(string date)
	{
		var parsedDate = DateTime.Parse(date);

		var existing = _dbContext.PollinationCalendars
			.Any(x => x.Date == parsedDate);

		if (existing)
		{
			throw new BadRequestException("Already filled this day");
		}

		var regions = _dbContext.Regions.ToList();
		
		var inhalantAllergens = _dbContext.Allergens
			.Include(x => x.AllergenType)
			.Where(x => x.AllergenType.Name == "Wziewny")
			.ToList();

		var pollinationCalendars = new List<PollinationCalendar>();

		var rand = new Random();
		foreach (var region in regions)
		{
			foreach (var allergen in inhalantAllergens)
			{
				pollinationCalendars.Add(
					new PollinationCalendar
					{
						RegionId = region.Id,
						AllergenId = allergen.Id,
						Date = parsedDate,
						Strength = rand.Next(4)
					});	
			}
		}
		
		_dbContext.AddRange(pollinationCalendars);
		_dbContext.SaveChanges();
	}
}