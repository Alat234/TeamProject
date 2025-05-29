using Microsoft.EntityFrameworkCore;

using TeamProject.Models;

namespace TeamProject.DbContexts;

public class BmpEditorDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<ImageEntity> Images { get; set; } = null!;

    public BmpEditorDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired(false);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.Images)
                .WithOne()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<ImageEntity>(entity =>
        {
            entity.ToTable("Images");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ImageData).IsRequired(false);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.HasSecretText).IsRequired().HasDefaultValue(false);
        });
    }
}

