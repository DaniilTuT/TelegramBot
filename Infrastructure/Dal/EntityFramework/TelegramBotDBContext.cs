using Domain.Entities;
using Infrastructure.Dal.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

public class TelegramBotDbContext: DbContext
{
    public TelegramBotDbContext (DbContextOptions<TelegramBotDbContext> options) : base(options) { }
    public DbSet<Person> Persons { get; set; }
    public DbSet<CustomField<string>> CustomFields { get; set; }
    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring (optionsBuilder);
        optionsBuilder.UseNpgsql(connectionString: "User ID=postgres;Password=1234; Host=localhost; Port=5432;Database=new_back;");
    }
    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating (modelBuilder);
        modelBuilder.ApplyConfiguration (new PersonConfiguration());
    }
}
