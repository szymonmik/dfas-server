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

		CreateMap<User, UserDto>()
			.ForMember(p => p.Allergens, opt => opt.MapFrom(c => c.UserAllergens.Where(a => a.UserId == c.Id).Select(a => a.Allergen)));

		CreateMap<Symptom, SymptomDto>();

		CreateMap<Entry, EntryDto>()
			.ForMember(p => p.Products, opt => opt.MapFrom(c => c.EntryProducts.Where(a => a.EntryId == c.Id).Select(a => a.Product)))
			.ForMember(p => p.Symptoms, opt => opt.MapFrom(c => c.EntrySymptoms.Where(a => a.EntryId == c.Id).Select(a => a.Symptom)));

		
		
		CreateMap<Allergen, AllergenDto>();

		CreateMap<CreateAllergenDto, Allergen>();
	}
}