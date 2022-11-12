using server.Entities;
using server.Exceptions;

namespace server.Services;

public class RegionService : IRegionService
{
	private readonly AppDbContext _dbContext;

	public RegionService(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public IEnumerable<Region> GetAll()
	{
		var regions = _dbContext.Regions.ToList();

		return regions;
	}

	public Region GetById(int id)
	{
		var region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

		if (region is null)
		{
			throw new NotFoundException("Region not found");
		}

		return region;
	}
	
}