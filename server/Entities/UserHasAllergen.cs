namespace server.Entities;

public class UserHasAllergen
{
	public int Id { get; set; }
	
	public int UserId { get; set; }
	public virtual User User { get; set; }
	
	public int AllergenId { get; set; }
	public virtual Allergen Allergen { get; set; }
}