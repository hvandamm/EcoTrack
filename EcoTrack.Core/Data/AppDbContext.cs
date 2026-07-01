using EcoTrack.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoTrack.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<TelemetryRecord> TelemetryRecords => Set<TelemetryRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasIndex(a => a.SerialNumber).IsUnique();
            entity.HasIndex(a => a.Name);

            entity.HasIndex(a => a.Status);
        });

        modelBuilder.Entity<TelemetryRecord>(entity =>
        {
            entity.HasIndex(tr => new { tr.AssetId, tr.Timestamp });

            entity.HasIndex(tr => tr.MetricName);

            entity
                .HasOne(tr => tr.Asset)
                .WithMany(a => a.TelemetryRecords)
                .HasForeignKey(tr => tr.AssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}