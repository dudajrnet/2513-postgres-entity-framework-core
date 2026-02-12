using Microsoft.EntityFrameworkCore;
using Poststore.Models;

namespace PostStore.Data;

public class PostStoreDbContext(DbContextOptions<PostStoreDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products {get; set;} = null!;
    public DbSet<Category> Categories {get; set;} = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}