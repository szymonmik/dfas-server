using server.Entities;
using server.Exceptions;

namespace server.Services;

public class SymptomService : ISymptomService
{
	private readonly AppDbContext _dbContext;
	
	public SymptomService(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	
	public IEnumerable<Symptom> GetAll()
	{
		var symptoms = _dbContext.Symptoms.ToList();

		return symptoms;
	}

	public Symptom GetById(int id)
	{
		var symptom = _dbContext.Symptoms.FirstOrDefault(r => r.Id == id);

		if (symptom is null)
		{
			throw new NotFoundException("Symptom not found");
		}

		return symptom;
	}
}