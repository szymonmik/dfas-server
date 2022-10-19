using Microsoft.EntityFrameworkCore;

namespace server.Entities;

public class AppDbContext : DbContext
{
    //Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
    //private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=AllergyDiaryDb;Trusted_Connection=True;";

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

	public DbSet<Product> Products { get; set; }
	
	public DbSet<Allergen> Allergens { get; set; }
	public DbSet<AllergenType> AllergenTypes { get; set; }
	
	public DbSet<User> Users { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<Region> Regions { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
			.Property(u => u.Email)
			.IsRequired();
		
		modelBuilder.Entity<Role>()
			.Property(u => u.Name)
			.IsRequired();
		
		modelBuilder.Entity<Product>()
			.Property(u => u.Name)
			.IsRequired();
		
		modelBuilder.Entity<Allergen>()
			.Property(u => u.Name)
			.IsRequired();
		
		modelBuilder.Entity<AllergenType>()
			.Property(u => u.Name)
			.IsRequired();
	}

	/*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
	{
		optionsBuilder.UseSqlServer(); // change to postgresql later
	}*/
}