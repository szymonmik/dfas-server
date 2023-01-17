using System.Security.Claims;
using server.Entities;

namespace server.Services;

public interface IPollinationService
{
	IEnumerable<PollinationCalendar> GetByDate(int regionId, string date);
	void FillRandomOnDate(string date);
}