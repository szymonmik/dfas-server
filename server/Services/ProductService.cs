using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using server.Authorization;
using server.Entities;
using server.Exceptions;
using server.Models;

namespace server.Services;

public class ProductService : IProductService
{
	private readonly AppDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IAuthorizationService _authorizationService;

	public ProductService(AppDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_authorizationService = authorizationService;
	}

	public IEnumerable<ProductDto> GetAll(int userId)
	{
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var products = _dbContext.Products
			.Include(x => x.ProductAllergens)
			.ThenInclude(x => x.Allergen)
			.ThenInclude(x => x.AllergenType)
			.Where(x => x.UserId == userId || x.UserId == null)
			.ToList();

		var productDtos = _mapper.Map<List<ProductDto>>(products);

		return productDtos;
	}
	
	public IEnumerable<ProductDto> GetAllCurrentUser(int userId)
	{
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}
		
		var products = _dbContext.Products
			.Include(x => x.ProductAllergens)
			.ThenInclude(x => x.Allergen)
			.ThenInclude(x => x.AllergenType)
			.Where(x => x.UserId == userId)
			.ToList();

		var productDtos = _mapper.Map<List<ProductDto>>(products);

		return productDtos;
	}

	public ProductDto GetById(int userId, int productId, ClaimsPrincipal userPrincipal)
	{
		var product = _dbContext.Products
			.Include(p => p.ProductAllergens)
			.ThenInclude(x => x.Allergen)
			.ThenInclude(x => x.AllergenType)
			.FirstOrDefault(p => p.Id == productId);

		if (product is null)
		{
			throw new NotFoundException("Nie znaleziono produktu");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product, 
			new ResourceOperationRequirement(ResourceOperation.Read)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}
		
		var productDto = _mapper.Map<ProductDto>(product);

		return productDto;
	}

	public int CreateProduct(int userId, CreateProductDto dto)
	{
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("Nie znaleziono użytkownika");
		}

		var existingName = _dbContext.Products.FirstOrDefault(p => p.Name == dto.Name && p.UserId == userId);

		if (existingName != null)
		{
			throw new BadRequestException("Produkt o tej nazwie już istnieje");
		}

		var product = _mapper.Map<Product>(dto);
		product.UserId = userId;
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		return product.Id;
	}
	
	public void UpdateProduct(int userId, int productId, UpdateProductDto dto, ClaimsPrincipal userPrincipal)
	{
		var product = _dbContext.Products
			.Include(p => p.ProductAllergens)
			.FirstOrDefault(p => p.Id == productId);

		if (product is null)
		{
			throw new NotFoundException("Nie znaleziono produktu");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}
		
		var existingName = _dbContext.Products.FirstOrDefault(p => p.Name == dto.Name && p.UserId == userId);

		if (existingName != null)
		{
			throw new BadRequestException("Produkt o tej nazwie już istnieje");
		}

		product.Name = dto.Name;
		
		_dbContext.SaveChanges();
	}
	
	public void DeleteProduct(int productId, ClaimsPrincipal userPrincipal)
	{
		var product = _dbContext.Products
			.Include(p => p.ProductAllergens)
			.FirstOrDefault(p => p.Id == productId);

		if (product is null)
		{
			throw new NotFoundException("Nie znaleziono produktu");
		}

		var authorizationResult = _authorizationService
			.AuthorizeAsync(userPrincipal, product, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		_dbContext.Remove(product);
		_dbContext.SaveChanges();
		
	}

	public void AssignAllergen(int productId, int allergenId, ClaimsPrincipal userPrincipal)
	{
		var product = _dbContext.Products
			.Include(p => p.ProductAllergens)
			.FirstOrDefault(p => p.Id == productId);

		if (product is null)
		{
			throw new NotFoundException("Nie znaleziono produktu");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product,
			new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}
		
		var existingAssignment = _dbContext.ProductHasAllergens
			.FirstOrDefault(x => x.ProductId == productId && x.AllergenId == allergenId);

		if (existingAssignment != null)
		{
			throw new BadRequestException("Już przypisano");
		}
		
		var assignment = new ProductHasAllergen()
		{
			ProductId = productId,
			AllergenId = allergenId
		};
		
		_dbContext.ProductHasAllergens.Add(assignment);
		_dbContext.SaveChanges();
	}

	public void UnassignAllergen(int productId, int allergenId, ClaimsPrincipal userPrincipal)
	{
		var product = _dbContext.Products
			.Include(p => p.ProductAllergens)
			.FirstOrDefault(p => p.Id == productId);

		if (product is null)
		{
			throw new NotFoundException("Nie znaleziono produktu");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Zabronione dla tego użytkownika");
		}

		var existingAssignment = _dbContext.ProductHasAllergens
			.FirstOrDefault(x => x.ProductId == productId && x.AllergenId == allergenId);

		if (existingAssignment is null)
		{
			throw new BadRequestException("Przypisanie nie istnieje");
		}

		_dbContext.Remove(existingAssignment);
		_dbContext.SaveChanges();
	}
	
	public IEnumerable<ProductDto> GetFiltered()
	{
		var products = _dbContext.Products
			.Include(p => p.ProductAllergens)
			.ThenInclude(x => x.Allergen)
			.ThenInclude(x => x.AllergenType);

		List<String> filters = new List<string>();
		filters.Add("Gluten");
		filters.Add("Seler");

		if (products is null)
		{
			throw new NotFoundException("Nie znaleziono produktu");
		}
		
		//var filteredProducts = products.Where(p => p.ProductAllergens.All(c => filters.ToList().Contains(c.Allergen.Name)));
		//var filteredProducts = products.Where(p => filters.All(al => p.ProductAllergens.Any(c => c.Allergen.Name.Contains(al))));
		var filteredProducts = products.Where(p => p.ProductAllergens.Any(c => filters.Contains(c.Allergen.Name)));
		//var filteredProducts = products.Where(p => p.ProductAllergens.Any(c => p.ProductAllergens));
		
		var productDtos = _mapper.Map<List<ProductDto>>(filteredProducts);

		return productDtos;
	}
}