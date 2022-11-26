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
	
	public DbSet<ProductHasAllergen> ProductHasAllergens { get; set; }
	
	public DbSet<UserHasAllergen> UserHasAllergens { get; set; }

	public DbSet<Symptom> Symptoms { get; set; }
	
	public DbSet<Entry> Entries { get; set; }
	public DbSet<EntryHasProduct> EntryHasProducts { get; set; }
	public DbSet<EntryHasSymptom> EntryHasSymptoms { get; set; }
	
	public DbSet<PollinationCalendar> PollinationCalendars { get; set; }
	

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
		
		modelBuilder.Entity<Entry>()
			.Property(u => u.Date)
			.IsRequired();
		
		// PRODUCT - ALLERGEN RELATIONSHIP
		modelBuilder.Entity<ProductHasAllergen>()
			.HasKey(x => new { x.ProductId, x.AllergenId });

		modelBuilder.Entity<ProductHasAllergen>()
			.HasOne(x => x.Product)
			.WithMany(x => x.ProductAllergens)
			.HasForeignKey(x => x.ProductId);

		modelBuilder.Entity<ProductHasAllergen>()
			.HasOne(x => x.Allergen)
			.WithMany()
			.HasForeignKey(x => x.AllergenId);

		modelBuilder.Entity<Product>()
			.HasMany(x => x.ProductAllergens)
			.WithOne(x => x.Product)
			.HasForeignKey(x => x.ProductId);
		
		// USER - ALLERGEN RELATIONSHIP
		modelBuilder.Entity<UserHasAllergen>()
			.HasKey(x => new { x.UserId, x.AllergenId });

		modelBuilder.Entity<UserHasAllergen>()
			.HasOne(x => x.User)
			.WithMany(x => x.UserAllergens)
			.HasForeignKey(x => x.UserId);

		modelBuilder.Entity<UserHasAllergen>()
			.HasOne(x => x.Allergen)
			.WithMany()
			.HasForeignKey(x => x.AllergenId);

		modelBuilder.Entity<User>()
			.HasMany(x => x.UserAllergens)
			.WithOne(x => x.User)
			.HasForeignKey(x => x.UserId);
		
		// ENTRY - PRODUCT RELATIONSHIP
		modelBuilder.Entity<EntryHasProduct>()
			.HasKey(x => new { x.EntryId, x.ProductId });

		modelBuilder.Entity<EntryHasProduct>()
			.HasOne(x => x.Entry)
			.WithMany(x => x.EntryProducts)
			.HasForeignKey(x => x.EntryId);

		modelBuilder.Entity<EntryHasProduct>()
			.HasOne(x => x.Product)
			.WithMany()
			.HasForeignKey(x => x.ProductId);

		modelBuilder.Entity<Entry>()
			.HasMany(x => x.EntryProducts)
			.WithOne(x => x.Entry)
			.HasForeignKey(x => x.EntryId);
		
		// ENTRY - SYMPTOM RELATIONSHIP
		modelBuilder.Entity<EntryHasSymptom>()
			.HasKey(x => new { x.EntryId, x.SymptomId });

		modelBuilder.Entity<EntryHasSymptom>()
			.HasOne(x => x.Entry)
			.WithMany(x => x.EntrySymptoms)
			.HasForeignKey(x => x.EntryId);

		modelBuilder.Entity<EntryHasSymptom>()
			.HasOne(x => x.Symptom)
			.WithMany()
			.HasForeignKey(x => x.SymptomId);

		modelBuilder.Entity<Entry>()
			.HasMany(x => x.EntrySymptoms)
			.WithOne(x => x.Entry)
			.HasForeignKey(x => x.EntryId);
		
		// POLLINATION CALLENDAR
		
	}

	/*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
	{
		optionsBuilder.UseSqlServer(); // change to postgresql later
	}*/
}