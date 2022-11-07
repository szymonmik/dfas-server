using AutoMapper;
using server.Entities;
using server.Models;

namespace server;

public class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<CreateProductDto, Product>();

		CreateMap<Product, ProductDto>()
			.ForMember(p => p.Allergens, opt => opt.MapFrom(c => c.ProductAllergens.Where(a => a.ProductId == c.Id).Select(a => a.Allergen)));

		CreateMap<Allergen, AllergenDto>();

		CreateMap<CreateAllergenDto, Allergen>();
	}
}