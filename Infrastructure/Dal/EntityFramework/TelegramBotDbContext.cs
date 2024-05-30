using Domain.Entities;
using Infrastructure.Dal.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.EntityFramework;

/// <summary>
/// Контекст базы данных
/// </summary>
public class TelegramBotDbContext : DbContext
{
    public DbSet<Person> persons { get; init; }
    public DbSet<CustomField<string>> CustomFields { get; init; }

    public TelegramBotDbContext(DbContextOptions<TelegramBotDbContext> options, IConfiguration configuration) : base(options)
    {
    }
    
    /// <summary>
    /// Метод применения конфигураций
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new CustomFieldConfiguration());
    }
}
