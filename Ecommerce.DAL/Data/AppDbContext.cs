using Ecommerce.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DAL.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     base.OnModelCreating(builder);
        //     builder
        //         .Entity<Product>()
        //         .HasOne(p => p.Category)
        //         .WithMany(c => c.Products)
        //         .HasForeignKey(p => p.CategoryId)
        //         .OnDelete(DeleteBehavior.Cascade);

        //     builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
        // }
    }
}
