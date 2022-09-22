namespace server.Entities;

public class PollinationCalendar
{
	public int Id { get; set; }
	
	public int VoivodeshipId { get; set; }
	public virtual Voivodeship Voivodeship { get; set; }
	
	public int CalendarId { get; set; }
	public virtual Calendar Calendar { get; set; }
	
	public int AllergenId { get; set; }
	public virtual Allergen Allergen { get; set; }
}