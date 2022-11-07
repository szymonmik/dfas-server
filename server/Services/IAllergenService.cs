using server.Entities;
using server.Models;

namespace server.Services;

public interface IAllergenService
{
	IEnumerable<AllergenDto> GetAllAllergens();
	AllergenDto GetAllergenById(int allergenId);
	IEnumerable<AllergenType> GetAllAllergenTypes();
	AllergenType GetAllergenTypeById(int allergenTypeId);
}