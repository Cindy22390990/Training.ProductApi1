using Microsoft.EntityFrameworkCore;
using Training.ProductApi1.Models;

namespace Training.ProductApi1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Bom>Boms { get; set; }
    public DbSet<History> Histories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Bom>()
            .HasIndex(b => new { b.ProductId, b.MaterialId })
            .IsUnique();

        modelBuilder.Entity<Bom>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Bom>()
            .HasOne(b => b.Product)
            .WithMany(p => p.Boms)
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Bom>()
            .HasOne(b => b.Material)
            .WithMany(m => m.Boms)
            .HasForeignKey(b => b.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}