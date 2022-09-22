using server.Entities;

namespace server;

public class DataSeeder
{
	private readonly AppDbContext _dbContext;

	public DataSeeder(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void Seed()
	{
		if (_dbContext.Database.CanConnect())
		{
			if (!_dbContext.Roles.Any())
			{
				var roles = GetRoles();
				_dbContext.Roles.AddRange(roles);
				_dbContext.SaveChanges();
			}
			
			if (!_dbContext.AllergenTypes.Any())
			{
				var allergenTypes = GetAllergenTypes();
				_dbContext.AllergenTypes.AddRange(allergenTypes);
				_dbContext.SaveChanges();
			}
			
			if (!_dbContext.Allergens.Any())
			{
				var allergens = GetAllergens();
				_dbContext.Allergens.AddRange(allergens);
				_dbContext.SaveChanges();
			}
		}
	}

	private IEnumerable<Role> GetRoles()
	{
		var roles = new List<Role>()
		{
			new Role()
			{
				Name = "User"
			},
			new Role()
			{
				Name = "Admin"
			}
		};

		return roles;
	}
	
	private IEnumerable<AllergenType> GetAllergenTypes()
	{
		var allergenTypes = new List<AllergenType>()
		{
			new AllergenType()
			{
				Name = "Pokarmowy"
			},
			new AllergenType()
			{
				Name = "Wziewny"
			}
		};

		return allergenTypes;
	}
	
	private IEnumerable<Allergen> GetAllergens()
	{
		var allergens = new List<Allergen>()
		{
			new Allergen()
			{
				Name = "Mleko",
				AllergenTypeId = 1
			},
			new Allergen()
			{
				Name = "Orzechy",
				AllergenTypeId = 1
			},
			new Allergen()
			{
				Name = "Seler",
				AllergenTypeId = 1
			},
			new Allergen()
			{
				Name = "Ryby",
				AllergenTypeId = 1
			},
			new Allergen()
			{
				Name = "Jaja",
				AllergenTypeId = 1
			},
			new Allergen()
			{
				Name = "Łubin",
				AllergenTypeId = 1
			},
			new Allergen()
			{
				Name = "Gorczyca",
				AllergenTypeId = 1
			},
			new Allergen()
			{
				Name = "Leszczyna",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Brzoza",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Brzoza",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Jesion",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Buk",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Grab",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Topola",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Platan",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Oliwka",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Cyprys",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Trawa",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Bylica",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Babka",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Komosa",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Pokrzywa",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Parietaria",
				AllergenTypeId = 2
			},
			new Allergen()
			{
				Name = "Ambrozja",
				AllergenTypeId = 2
			},
		};

		return allergens;
	}
}