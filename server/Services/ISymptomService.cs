using server.Entities;

namespace server.Services;

public interface ISymptomService
{
	IEnumerable<Symptom> GetAll();
	Symptom GetById(int id);
}