using Microsoft.EntityFrameworkCore;
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
			var pendingMigrations = _dbContext.Database.GetPendingMigrations();
			if(pendingMigrations != null && pendingMigrations.Any())
            {
	            _dbContext.Database.Migrate();
            }

			if (!_dbContext.Regions.Any())
			{
				var regions = GetRegions();
				_dbContext.Regions.AddRange(regions);
				_dbContext.SaveChanges();
			}
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
			
			if (!_dbContext.Products.Any())
			{
				var products = GetProducts();
				_dbContext.Products.AddRange(products);
				_dbContext.SaveChanges();
			}
			
			if (!_dbContext.ProductHasAllergens.Any())
			{
				var productAllergens = GetProductAllergens();
				_dbContext.ProductHasAllergens.AddRange(productAllergens);
				_dbContext.SaveChanges();
			}

			if (!_dbContext.Symptoms.Any())
			{
				var symptoms = GetSymptoms();
				_dbContext.Symptoms.AddRange(symptoms);
				_dbContext.SaveChanges();
			}
		}
	}

	private IEnumerable<Region> GetRegions()
	{
		var regions = new List<Region>()
		{
			new Region()
			{
				Name = "Region 1"
			},
			new Region()
			{
				Name = "Region 2"
			},
			new Region()
			{
				Name = "Region 3"
			},
			new Region()
			{
				Name = "Region 4"
			}
		};

		return regions;
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
			new Allergen() // 1
			{
				Name = "Gluten",
				AllergenTypeId = 1
			},
			new Allergen() // 2
			{
				Name = "Mleko",
				AllergenTypeId = 1
			},
			new Allergen() // 3
			{
				Name = "Orzechy",
				AllergenTypeId = 1
			},
			new Allergen() // 4
			{
				Name = "Seler",
				AllergenTypeId = 1
			},
			new Allergen() // 5
			{
				Name = "Ryby",
				AllergenTypeId = 1
			},
			new Allergen() // 6
			{
				Name = "Jaja",
				AllergenTypeId = 1
			},
			new Allergen() // 7
			{
				Name = "Łubin",
				AllergenTypeId = 1
			},
			new Allergen() // 8
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
	
	private IEnumerable<Product> GetProducts()
	{
		var products = new List<Product>()
		{
			new Product()
			{
				Name = "Bułka pszenna"
			},
			new Product()
			{
				Name = "Jajko"
			},
			new Product()
			{
				Name = "Ryż biały"
			},
			new Product()
			{
				Name = "Ryż brązowy"
			},
			new Product()
			{
				Name = "Ryż basmati"
			},
			new Product()
			{
				Name = "Makaron pszenny"
			},
			new Product()
			{
				Name = "Makaron pełnoziarnisty"
			},
			new Product()
			{
				Name = "Masło"
			},
			new Product()
			{
				Name = "Mleko"
			}
		};

		return products;
	}

	private IEnumerable<ProductHasAllergen> GetProductAllergens()
	{
		var productAllergens = new List<ProductHasAllergen>()
		{
			new ProductHasAllergen()
			{
				ProductId = 1,
				AllergenId = 1
			},
			new ProductHasAllergen()
			{
				ProductId = 2,
				AllergenId = 6
			},
			new ProductHasAllergen()
			{
				ProductId = 3,
				AllergenId = 1
			},
			new ProductHasAllergen()
			{
				ProductId = 6,
				AllergenId = 1
			},
			new ProductHasAllergen()
			{
				ProductId = 6,
				AllergenId = 6
			},
			new ProductHasAllergen()
			{
				ProductId = 7,
				AllergenId = 1
			},
			new ProductHasAllergen()
			{
				ProductId = 7,
				AllergenId = 6
			},
			new ProductHasAllergen()
			{
				ProductId = 8,
				AllergenId = 2
			},
			new ProductHasAllergen()
			{
				ProductId = 9,
				AllergenId = 2
			},
			
		};

		return productAllergens;
	}

	private IEnumerable<Symptom> GetSymptoms()
	{
		var symptoms = new List<Symptom>()
		{
			new Symptom()
			{
				Name = "Wodnisty katar"
			},
			new Symptom()
			{
				Name = "Kichanie"
			},
			new Symptom()
			{
				Name = "Zatkany nos"
			},
			new Symptom()
			{
				Name = "Swędzenie gardła"
			},
			new Symptom()
			{
				Name = "Zaczerwienienie"
			},
			new Symptom()
			{
				Name = "Swędzenie"
			},
			new Symptom()
			{
				Name = "Łzawienie oczu"
			},
			new Symptom()
			{
				Name = "Kaszel"
			},
			new Symptom()
			{
				Name = "Świszczący oddech"
			},
			new Symptom()
			{
				Name = "Wymioty"
			},
			new Symptom()
			{
				Name = "Biegunka"
			}
		};

		return symptoms;
	}
}