﻿namespace server.Models;

public class UpdateUserPasswordDto
{
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
}