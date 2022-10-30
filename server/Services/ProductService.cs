using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
		
		var products = _dbContext.Products.Where(x => x.UserId == userId || x.UserId == null).ToList();

		var productDtos = _mapper.Map<List<ProductDto>>(products);

		return productDtos;
	}

	public ProductDto GetById(int userId, int productId, ClaimsPrincipal userPrincipal)
	{
		var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

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

		var product = _mapper.Map<Product>(dto);
		product.UserId = userId;
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		return product.Id;
	}
}