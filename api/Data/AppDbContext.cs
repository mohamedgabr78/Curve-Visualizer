using System;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<CurveModel> Curves { get; set; } = null!;
        public DbSet<PointModel> Points { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurveModel>()
                .HasMany(c => c.Points)
                .WithOne(p => p.Curve)
                .HasForeignKey(p => p.CurveId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}