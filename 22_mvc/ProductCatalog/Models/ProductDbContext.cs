using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Models;

public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public ProductDbContext(DbContextOptions<ProductDbContext> options) :
        base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Price = 50000, Description = "Game laptop"},
            new Product {Id = 2, Name = "Smartphone", Price = 20000, Description = "Nice smartphone"},
            new Product {Id = 3, Name = "Headphone", Price = 4000, Description = "Headphone description"}
        );
    }
}
