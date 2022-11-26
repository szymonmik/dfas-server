using System.Security.Claims;
using server.Entities;

namespace server.Services;

public interface IPollinationService
{
	IEnumerable<PollinationCalendar> GetByDate(int regionId, string date, ClaimsPrincipal userPrincipal);
	void FillRandomOnDate(string date);
}