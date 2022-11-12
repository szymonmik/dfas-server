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
			throw new NotFoundException("User not found");
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
			throw new NotFoundException("User not found");
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
			throw new NotFoundException("Product not found");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product, new ResourceOperationRequirement(ResourceOperation.Read)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Forbidden for this user");
		}
		
		var productDto = _mapper.Map<ProductDto>(product);

		return productDto;
	}

	public int CreateProduct(int userId, CreateProductDto dto)
	{
		var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

		if (user is null)
		{
			throw new NotFoundException("User not found");
		}

		var existingName = _dbContext.Products.FirstOrDefault(p => p.Name == dto.Name && p.UserId == userId);

		if (existingName != null)
		{
			throw new BadRequestException("Product with this name already exists");
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
			throw new NotFoundException("Product not found");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Forbidden for this user");
		}
		
		var existingName = _dbContext.Products.FirstOrDefault(p => p.Name == dto.Name && p.UserId == userId);

		if (existingName != null)
		{
			throw new BadRequestException("Product with this name already exists");
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
			throw new NotFoundException("Product not found");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Forbidden for this user");
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
			throw new NotFoundException("Product not found");
		}
		
		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Forbidden for this user");
		}

		ProductHasAllergen assignment = new ProductHasAllergen()
		{
			ProductId = productId,
			AllergenId = allergenId
		};

		var existingAssignment = _dbContext.ProductHasAllergens.FirstOrDefault(x => x.ProductId == productId && x.AllergenId == allergenId);

		if (existingAssignment != null)
		{
			throw new BadRequestException("Already assigned");
		}
		
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
			throw new NotFoundException("Product not found");
		}

		var authorizationResult = _authorizationService.AuthorizeAsync(userPrincipal, product, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

		if (!authorizationResult.Succeeded)
		{
			throw new ForbidException("Forbidden for this user");
		}

		var existingAssignment = _dbContext.ProductHasAllergens.FirstOrDefault(x => x.ProductId == productId && x.AllergenId == allergenId);

		if (existingAssignment is null)
		{
			throw new BadRequestException("Assignment does not exist");
		}

		_dbContext.Remove(existingAssignment);
		_dbContext.SaveChanges();
	}
}