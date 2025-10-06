using Kaizen.API.Extensions;
using Kaizen.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Data;

public class KaizenDbContext(DbContextOptions<KaizenDbContext> options) : IdentityDbContext<KaizenUser>(options)
{
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<WorkoutSet> WorkoutSets { get; set; }
    
    // Static reference data
    public DbSet<MuscleGroup> MuscleGroups { get; set; }
    public DbSet<MeasurementUnit> MeasurementUnits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Exercise>()
            .HasIndex(e => e.Name)
            .IsUnique();

        modelBuilder.Entity<MuscleGroup>(m =>
        {
            m.HasKey(e => e.Code);
            m.HasIndex(e => e.Name).IsUnique();
            m.HasData(DbContextExtensions.AppMuscleGroups);
        });

        // Unidirectional many-to-many relationship between exercises and muscles used.
        // Customize join table (ExerciseMuscleGroup) FK to point to MuscleGroup.Code.
        modelBuilder.Entity<Exercise>()
            .HasMany<MuscleGroup>(e => e.MuscleGroups)
            .WithMany()
            .UsingEntity<ExerciseMuscleGroup>(join => join
                .HasOne<MuscleGroup>()
                .WithMany()
                .HasForeignKey(emg => emg.MuscleGroupCode)
                .HasPrincipalKey(mg => mg.Code));

        modelBuilder.Entity<MeasurementUnit>(e =>
        {
            e.HasKey(mu => mu.Code);
            e.HasIndex(mu => mu.Name).IsUnique();
            e.HasData(DbContextExtensions.AppMeasurementUnits);
        });
        
        modelBuilder.Entity<WorkoutSet>(e =>
        {
            e.Property(ws => ws.Quantity).HasColumnType("decimal(6,2)");
            
            // When exercises and unit entities are deleted (which the app doesn't allow anyway),
            // they can only be done so if there are no workout sets referencing the entities (RESTRICT).

            e.HasOne<MeasurementUnit>(ws => ws.MeasurementUnit)
                .WithMany()
                .HasForeignKey(w => w.MeasurementUnitCode)
                .HasPrincipalKey(mu => mu.Code)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<Exercise>(ws => ws.Exercise)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        });
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