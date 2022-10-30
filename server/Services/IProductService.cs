using System.Security.Claims;
using server.Models;

namespace server.Services;

public interface IProductService
{
	IEnumerable<ProductDto> GetAll(int id);

	ProductDto GetById(int userId, int productId, ClaimsPrincipal userPrincipal);

	int CreateProduct(int id, CreateProductDto dto);

}