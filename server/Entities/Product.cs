namespace server.Entities;

public class Product
{
	public int Id { get; set; }
	public string Name { get; set; }
	public bool IsDeleted { get; set; }
	
	public int? UserId { get; set; }
	public virtual User User { get; set; }
	
	public IEnumerable<ProductHasAllergen> ProductAllergens { get; set; }
}

