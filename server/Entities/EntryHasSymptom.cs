namespace server.Entities;

public class EntryHasSymptom
{
	public int EntryId { get; set; }
	public virtual Entry Entry { get; set; }
	
	public int SymptomId { get; set; }
	public virtual Symptom Symptom { get; set; }
}