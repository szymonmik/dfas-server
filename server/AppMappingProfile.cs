using AutoMapper;
using server.Entities;
using server.Models;

namespace server;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<CreateProductDto, Product>();

        CreateMap<CreateAllergenDto, Allergen>();

        /*CreateMap<Product, ProductDto>()
            .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
            .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
            .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));*/

        /*
        CreateMap<Dish, DishDto>();
        */

        /*
        CreateMap<CreateProductDto, Product>()
            .ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
            {
                City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street 
            }));
            */
    }
}