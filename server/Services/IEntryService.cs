using System.Security.Claims;
using server.Models;

namespace server.Services;

public interface IEntryService
{
	IEnumerable<EntryDto> GetAllCurrentUser(ClaimsPrincipal userPrincipal);
	EntryDto GetById(int entryId, ClaimsPrincipal userPrincipal);
	EntryDto GetByDate(string entryDate, ClaimsPrincipal userPrincipal);
	//int CreateEntry(CreateEntryDto dto, ClaimsPrincipal userPrincipal);
	int CreateEmptyEntry(CreateEntryDto dto, ClaimsPrincipal userPrincipal);
	void DeleteEntry(string entryDate, ClaimsPrincipal userPrincipal);
	void AssignProduct(string entryDate, int productId, ClaimsPrincipal userPrincipal);
	void UnassignProduct(string entryDate, int productId, ClaimsPrincipal userPrincipal);
	void AssignSymptom(string entryDate, int symptomId, ClaimsPrincipal userPrincipal);
	void UnassignSymptom(string entryDate, int symptomId, ClaimsPrincipal userPrincipal);
}