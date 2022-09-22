namespace server.Entities;

public class DayHasProduct
{
	public int Id { get; set; }
	
	public int DayId { get; set; }
	public virtual Day Day { get; set; }
	
	public int ProductId { get; set; }
	public virtual Product Product { get; set; }
}