﻿namespace server.Entities;

public class PasswordResetToken
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public string Token { get; set; }
}