
namespace server.Entities;

public class PollinationCalendar
{
	public int Id { get; set; }
	
	public int RegionId { get; set; }
	public virtual Region Region { get; set; }

	public int AllergenId { get; set; }
	public virtual Allergen Allergen { get; set; }
	
	public DateTime Date { get; set; }

	public int Strength { get; set; }
}