namespace server.Models;

public class EntryDto
{
	public int Id { get; set; }
	
	public DateTime? Date { get; set; }

	public IEnumerable<ProductDto> Products { get; set; }
	public IEnumerable<SymptomDto> Symptoms { get; set; }
}