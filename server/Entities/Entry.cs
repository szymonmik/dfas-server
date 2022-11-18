namespace server.Entities;

public class Entry
{
	public int Id { get; set; }
	
	public DateTime? Date { get; set; }

	public int UserId { get; set; }
	public virtual User User { get; set; }
	
	public IEnumerable<EntryHasProduct> EntryProducts { get; set; }
	public IEnumerable<EntryHasSymptom> EntrySymptoms { get; set; }
}