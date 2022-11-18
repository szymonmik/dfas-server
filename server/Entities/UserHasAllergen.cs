namespace server.Entities;

public class UserHasAllergen
{
	public int UserId { get; set; }
	public User User { get; set; }
	
	public int AllergenId { get; set; }
	public Allergen Allergen { get; set; }
}