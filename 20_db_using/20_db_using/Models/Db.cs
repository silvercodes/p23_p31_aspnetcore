using Microsoft.EntityFrameworkCore;

namespace _20_db_using.Models;

public class Db : DbContext
{
    public Db(DbContextOptions<Db> options) 
        : base(options)
    { }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Title)
            .HasMaxLength(100);

        modelBuilder.Entity<TodoItem>()
            .HasIndex(t => t.Title);

        // ......
    }
}
