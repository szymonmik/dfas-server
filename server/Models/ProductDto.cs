﻿using server.Entities;

namespace server.Models;

public class ProductDto
{
	public int Id { get; set; }
	public string Name { get; set; }

	public IEnumerable<AllergenDto> Allergens { get; set; }
}