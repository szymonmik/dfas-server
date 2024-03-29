﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using server.Authorization;
using server.Entities;
using server.Exceptions;
using server.Models;

namespace server.Services;

public class EntryService : IEntryService
{
	private readonly AppDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IAuthorizationService _authorizationService;

	public EntryService(AppDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_authorizationService = authorizationService;
	}

	public IEnumerable<EntryDto> GetAllCurrentUser(ClaimsPrincipal userPrincipal)
	{
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var entries = _dbContext.Entries
			.Include(x => x.EntryProducts)
			.Include(x => x.EntrySymptoms)
			.Where(x => x.UserId == userId)
			.ToList();

		var entryDtos = _mapper.Map<List<EntryDto>>(entries);

		return entryDtos;
	}

	public EntryDto GetById(int entryId, ClaimsPrincipal userPrincipal)
	{
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var entry = _dbContext.Entries
			.Include(x => x.EntryProducts)
			.Include(x => x.EntrySymptoms)
			.FirstOrDefault(x => x.Id == entryId);
		
		if (entry is null)
		{
			throw new NotFoundException("Nie znaleziono wpisu");
		}
		
		var entryDto = _mapper.Map<EntryDto>(entry);

		return entryDto;
	}

	public EntryDto GetByDate(string entryDate, ClaimsPrincipal userPrincipal)
	{
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var parsedDate = DateTime.Parse(entryDate);

		var entry = _dbContext.Entries
			.Include(x => x.EntryProducts)
			.ThenInclude(x => x.Product)
			.ThenInclude(x => x.ProductAllergens)
			.ThenInclude(x => x.Allergen)
			.ThenInclude(x => x.AllergenType)
			.Include(x => x.EntrySymptoms)
			.ThenInclude(x => x.Symptom)
			.FirstOrDefault(x => x.UserId == userId && x.Date.Value == parsedDate);

		if (entry is null)
		{
			throw new NotFoundException("Nie znaleziono wpisu");
		}
		
		var entryDto = _mapper.Map<EntryDto>(entry);
		
		return entryDto;
	}

	public int CreateEmptyEntry(CreateEntryDto dto, ClaimsPrincipal userPrincipal)
	{
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		if (dto.Date is null)
		{
			throw new BadRequestException("Nie podano daty");
		}

		var parsedDate = DateTime.Parse(dto.Date);

		var existingEntry = _dbContext.Entries.FirstOrDefault(x => x.UserId == userId && x.Date.Value == parsedDate);
		if (existingEntry != null)
		{
			throw new BadRequestException("Wpis na dzisiaj dla bieżącego użytkownika już istnieje");
		}

		var newEntry = new Entry
		{
			Date = parsedDate,
			UserId = userId
		};

		_dbContext.Entries.Add(newEntry);
		_dbContext.SaveChanges();
		
		return newEntry.Id;
	}

	public void DeleteEntry(string entryDate, ClaimsPrincipal userPrincipal)
	{
		if (entryDate is null)
		{
			throw new BadRequestException("Nie podano daty");
		}

		var parsedDate = DateTime.Parse(entryDate);
		
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var entry = _dbContext.Entries
			.Include(x => x.EntryProducts)
			.Include(x => x.EntrySymptoms)
			.FirstOrDefault(x => x.UserId == userId && x.Date.Value == parsedDate);

		if (entry is null)
		{
			throw new NotFoundException("Nie znaleziono wpisu");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, entry, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		_dbContext.Remove(entry);
		_dbContext.SaveChanges();
	}

	public void AssignProduct(string entryDate, int productId, ClaimsPrincipal userPrincipal)
	{
		if (entryDate is null)
		{
			throw new BadRequestException("Nie podano daty");
		}
		
		var parsedDate = DateTime.Parse(entryDate);
		
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
		
		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var entry = _dbContext.Entries
			.Include(x => x.EntryProducts)
			.Include(x => x.EntrySymptoms)
			.FirstOrDefault(x => x.UserId == userId && x.Date.Value == parsedDate);

		if (entry is null)
		{
			throw new NotFoundException("Nie znaleziono wpisu");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, entry, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}
		
		var existingAssignment = _dbContext.EntryHasProducts.FirstOrDefault(x => x.EntryId == entry.Id && x.ProductId == productId);

		if (existingAssignment != null)
		{
			throw new BadRequestException("Już przypisano");
		}

		var assignment = new EntryHasProduct()
		{
			EntryId = entry.Id,
			ProductId = productId
		};
		
		_dbContext.EntryHasProducts.Add(assignment);
		_dbContext.SaveChanges();
	}

	public void UnassignProduct(string entryDate, int productId, ClaimsPrincipal userPrincipal)
	{
		if (entryDate is null)
		{
			throw new BadRequestException("Nie podano daty");
		}
		
		var parsedDate = DateTime.Parse(entryDate);
		
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
		
		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var entry = _dbContext.Entries
			.Include(x => x.EntryProducts)
			.Include(x => x.EntrySymptoms)
			.FirstOrDefault(x => x.UserId == userId && x.Date.Value == parsedDate);

		if (entry is null)
		{
			throw new NotFoundException("Nie znaleziono wpisu");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, entry, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		var existingAssignment = _dbContext.EntryHasProducts.FirstOrDefault(x => x.EntryId == entry.Id && x.ProductId == productId);

		if (existingAssignment is null)
		{
			throw new BadRequestException("Przypisanie nie istnieje");
		}

		_dbContext.Remove(existingAssignment);
		_dbContext.SaveChanges();
	}

	public void AssignSymptom(string entryDate, int symptomId, ClaimsPrincipal userPrincipal)
	{
		if (entryDate is null)
		{
			throw new BadRequestException("Nie podano daty");
		}
		
		var parsedDate = DateTime.Parse(entryDate);
		
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
		
		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var entry = _dbContext.Entries
			.Include(x => x.EntryProducts)
			.Include(x => x.EntrySymptoms)
			.FirstOrDefault(x => x.UserId == userId && x.Date.Value == parsedDate);

		if (entry is null)
		{
			throw new NotFoundException("Nie znaleziono wpisu");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, entry, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}
		
		var existingAssignment = _dbContext.EntryHasSymptoms.FirstOrDefault(x => x.EntryId == entry.Id && x.SymptomId == symptomId);

		if (existingAssignment != null)
		{
			throw new BadRequestException("Już przypisano");
		}

		var assignment = new EntryHasSymptom()
		{
			EntryId = entry.Id,
			SymptomId = symptomId
		};
		
		_dbContext.EntryHasSymptoms.Add(assignment);
		_dbContext.SaveChanges();
	}

	public void UnassignSymptom(string entryDate, int symptomId, ClaimsPrincipal userPrincipal)
	{
		if (entryDate is null)
		{
			throw new BadRequestException("Nie podano daty");
		}
		
		var parsedDate = DateTime.Parse(entryDate);
		
		var userId = int.Parse(userPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
		
		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var entry = _dbContext.Entries
			.Include(x => x.EntryProducts)
			.Include(x => x.EntrySymptoms)
			.FirstOrDefault(x => x.UserId == userId && x.Date.Value == parsedDate);

		if (entry is null)
		{
			throw new NotFoundException("Nie znaleziono wpisu");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, entry, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		var existingAssignment = _dbContext.EntryHasSymptoms.FirstOrDefault(x => x.EntryId == entry.Id && x.SymptomId == symptomId);

		if (existingAssignment is null)
		{
			throw new BadRequestException("Przypisanie nie istnieje");
		}

		_dbContext.Remove(existingAssignment);
		_dbContext.SaveChanges();
	}
}