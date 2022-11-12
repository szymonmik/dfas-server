using server.Entities;

namespace server.Services;

public interface IRegionService
{
	IEnumerable<Region> GetAll();
	Region GetById(int id);
}