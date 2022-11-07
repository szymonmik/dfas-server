namespace server.Entities;

public class ProductHasAllergen
{
	public int ProductId { get; set; }
	public Product Product { get; set; }
	
	public int AllergenId { get; set; }
	public Allergen Allergen { get; set; }
}