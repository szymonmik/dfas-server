namespace server.Entities;

public class ProductHasAllergen
{
	public int Id { get; set; }
	
	public int ProductId { get; set; }
	public virtual Product Product { get; set; }
	
	public int AllergenId { get; set; }
	public virtual Allergen Allergen { get; set; }
}