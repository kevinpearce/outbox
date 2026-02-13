using Microsoft.EntityFrameworkCore;
using Outbox.Domain;

namespace Outbox.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired();
            entity.Property(u => u.Id).IsRequired();
            entity.Ignore(u => u.DomainEvents);
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(10000);
            entity.Property(e => e.OccurredOnUtc).IsRequired();
            entity.Property(e => e.ProcessedOnUtc);
            entity.Property(e => e.Error).HasMaxLength(2000);

            entity.HasIndex(e => new { e.ProcessedOnUtc, e.Error, e.OccurredOnUtc })
                .HasFilter("[ProcessedOnUtc] IS NULL AND [Error] IS NULL");
        });
    }
}