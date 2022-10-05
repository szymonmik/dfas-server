using AutoMapper;
using server.Entities;
using server.Exceptions;
using server.Models;

namespace server.Services;

public class ProductService : IProductService
{
	private readonly AppDbContext _dbContext;
	private readonly IMapper _mapper;

	public ProductService(AppDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public IEnumerable<ProductDto> GetAll()
	{
		var products = _dbContext.Products.ToList();

		var productDtos = _mapper.Map<List<ProductDto>>(products);

		return productDtos;
	}

	public ProductDto GetById(int id)
	{
		var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

		if (product is null)
		{
			throw new NotFoundException("Product not found");
		}
		
		var productDto = _mapper.Map<ProductDto>(product);

		return productDto;
	}
}