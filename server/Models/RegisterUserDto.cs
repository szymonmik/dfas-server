namespace server.Models;

public class RegisterUserDto
{
	public string Name { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
	public DateTime? BirthDate { get; set; }
	public char Sex { get; set; } = 'm';
	public int RoleId { get; set; } = 1;
	public int RegionId { get; set; } = 1;
}