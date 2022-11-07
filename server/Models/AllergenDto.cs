using server.Entities;

namespace server.Models;

public class AllergenDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	
	public int AllergenTypeId { get; set; }
	public AllergenType AllergenType { get; set; }
}