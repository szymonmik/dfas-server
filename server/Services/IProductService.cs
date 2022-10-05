using server.Models;

namespace server.Services;

public interface IProductService
{
	IEnumerable<ProductDto> GetAll();

	ProductDto GetById(int id);
	
}