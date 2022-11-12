using System.Security.Claims;
using server.Models;

namespace server.Services;

public interface IProductService
{
	IEnumerable<ProductDto> GetAll(int userId);
	IEnumerable<ProductDto> GetAllCurrentUser(int userId);
	ProductDto GetById(int userId, int productId, ClaimsPrincipal userPrincipal);
	int CreateProduct(int userId, CreateProductDto dto);
	void UpdateProduct(int userId, int productId, UpdateProductDto dto, ClaimsPrincipal userPrincipal);
	void DeleteProduct(int productId, ClaimsPrincipal userPrincipal);
	void AssignAllergen(int productId, int allergenId, ClaimsPrincipal userPrincipal);
	void UnassignAllergen(int productId, int allergenId, ClaimsPrincipal userPrincipal);
}