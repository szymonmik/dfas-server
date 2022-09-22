namespace server.Entities;

public class Day
{
	public int Id { get; set; }
	public string Comment { get; set; }
	
	public int CalendarId { get; set; }
	public virtual Calendar Calendar { get; set; }
	
	public int UserId { get; set; }
	public virtual User User { get; set; }
}