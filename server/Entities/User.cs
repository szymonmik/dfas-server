﻿namespace server.Entities;

public class User
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string PasswordHash { get; set; }
	public DateTime? BirthDate { get; set; }
	public char Sex { get; set; }
	public float Height { get; set; }
	public float Weight { get; set; }
	public bool IsDeleted { get; set; }

	public int RoleId { get; set; }
	public virtual Role Role { get; set; }
	
	public int VoivodeshipId { get; set; }
	public virtual Voivodeship Voivodeship { get; set; }
}