namespace YTShortsAlarm.Web.Data;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<WaitlistEntry> WaitlistEntries => Set<WaitlistEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WaitlistEntry>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => new { e.IpAddress, e.CreatedAtUtc });
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
        });
    }
}
