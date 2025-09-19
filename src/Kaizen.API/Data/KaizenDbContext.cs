using Kaizen.API.Extensions;
using Kaizen.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Data;

public class KaizenDbContext(DbContextOptions<KaizenDbContext> options) : IdentityDbContext<KaizenUser>(options)
{
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<MuscleGroup> MuscleGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseSeeding((dbContext, _) => { dbContext.SeedMuscleGroups(); })
            .UseAsyncSeeding(async (dbContext, _, cancellationToken) =>
            {
                await dbContext.SeedMuscleGroupsAsync(cancellationToken);
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MuscleGroup>(m =>
        {
            m.HasKey(e => e.Code);
            m.HasIndex(e => e.Name).IsUnique();
        });

        // Unidirectional many-to-many relationship between exercises and muscles used.
        // Customize join table (ExerciseMuscleGroup) FK to point to MuscleGroup.Code.
        modelBuilder.Entity<Exercise>()
            .HasMany<MuscleGroup>(e => e.MuscleGroups)
            .WithMany()
            .UsingEntity<ExerciseMuscleGroup>(join => join
                .HasOne<MuscleGroup>()
                .WithMany()
                .HasForeignKey(nameof(ExerciseMuscleGroup.MuscleGroupCode))
                .HasPrincipalKey(nameof(MuscleGroup.Code)));
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<TimestampedEntity>();

        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            var timestamp = DateTimeOffset.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = timestamp;
                entity.UpdatedAt = timestamp;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = timestamp;
            }
        }
    }
}