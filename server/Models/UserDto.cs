using server.Entities;

namespace server.Models;

public class UserDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public DateTime? BirthDate { get; set; }
	public char Sex { get; set; }
	
	public int RoleId { get; set; }
	public virtual Role Role { get; set; }
	
	public int RegionId { get; set; }
	public virtual Region Region { get; set; }

	public IEnumerable<AllergenDto> Allergens { get; set; }
}