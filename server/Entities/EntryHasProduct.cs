namespace server.Entities;

public class EntryHasProduct
{
	public int EntryId { get; set; }
	public virtual Entry Entry { get; set; }
	
	public int ProductId { get; set; }
	public virtual Product Product { get; set; }
}