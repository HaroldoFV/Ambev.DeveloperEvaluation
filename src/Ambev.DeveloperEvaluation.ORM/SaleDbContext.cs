using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ambev.DeveloperEvaluation.ORM;

public class SaleDbContext : DbContext
{
    public SaleDbContext(DbContextOptions<SaleDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
        {
            property.SetColumnType("varchar(100)");
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SaleDbContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        modelBuilder.HasSequence<int>("SaleSequence").StartsAt(1000).IncrementsBy(1);
        base.OnModelCreating(modelBuilder);
    }
}

public class SaleDbContextFactory : IDesignTimeDbContextFactory<SaleDbContext>
{
    public SaleDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var builder = new DbContextOptionsBuilder<SaleDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(typeof(SaleDbContext).Assembly.FullName)
        );

        return new SaleDbContext(builder.Options);
    }
}