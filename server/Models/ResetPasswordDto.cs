﻿namespace server.Models;

public class ResetPasswordDto
{
	public int UserId { get; set; }
	public string Token { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
}