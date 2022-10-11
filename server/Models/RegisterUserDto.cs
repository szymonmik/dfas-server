namespace server.Models;

public class RegisterUserDto
{
	public string Name { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
	public DateTime? BirthDate { get; set; }
	public int RoleId { get; set; } = 1;
	public int VoivodeshipId { get; set; }
}