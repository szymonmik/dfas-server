namespace server.Entities;

public class Allergen
{
	public int Id { get; set; }
	public string Name { get; set; }
	public bool IsDeleted { get; set; }
	
	public int AllergenTypeId { get; set; }
	public virtual AllergenType AllergenType { get; set; }
	}