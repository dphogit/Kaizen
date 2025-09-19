using Kaizen.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Extensions;

public static class DbContextExtensions
{
    public static DbContext SeedMuscleGroups(this DbContext dbContext)
    {
        // Enter muscles into the DB that are not already in it.
        var toAdd = GetMusclesGroupToAdd(dbContext);
        
        if (toAdd.Count > 0)
        {
            dbContext.AddRange(toAdd);
            dbContext.SaveChanges();
        }
        
        return dbContext;
    }

    public static async Task<DbContext> SeedMuscleGroupsAsync(
        this DbContext dbContext,
        CancellationToken cancellationToken)
    {
        var toAdd = GetMusclesGroupToAdd(dbContext);
        
        if (toAdd.Count > 0)
        {
            await dbContext.AddRangeAsync(toAdd, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
        return dbContext;
    }

    private static List<MuscleGroup> GetMusclesGroupToAdd(DbContext dbContext)
    {
        var dbCodes = dbContext.Set<MuscleGroup>().Select(x => x.Code);
        var toAdd = DefaultMuscleGroups.ExceptBy(dbCodes, m => m.Code).ToList();
        return toAdd;
    }
    
    // https://www.muscleandstrength.com/exercises
    private static readonly List<MuscleGroup> DefaultMuscleGroups =
    [
        // Core
        new() { Code = "abductors", Name = "Abductors" },
        new() { Code = "abs", Name = "Abdominal" },
        new() { Code = "adductors", Name = "Adductors" },
        new() { Code = "hip_flexors", Name = "Hip Flexors" },
        new() { Code = "obliques", Name = "Obliques" },
        
        // Arms
        new() { Code = "biceps", Name = "Biceps" },
        new() { Code = "triceps", Name = "Triceps" },
        new() { Code = "forearms", Name = "Forearms" },
        
        // Main body
        new() { Code = "traps", Name = "Trapezius" },
        new() { Code = "neck", Name = "Neck" },
        new() { Code = "chest", Name = "Chest" },
        new() { Code = "lower_back", Name = "Lower Back" },
        new() { Code = "upper_back", Name = "Upper Back" },
        new() { Code = "lats", Name = "Latissimus Dorsi" },
        new() { Code = "shoulders", Name = "Shoulders" },
        
        // legs
        new() { Code = "calves", Name = "Calves" },
        new() { Code = "glutes", Name = "Gluteus Maximus" },
        new() { Code = "hamstrings", Name = "Hamstrings" },
        new() { Code = "it_band", Name = "IT Band"},
        new() { Code = "quads", Name = "Quadriceps" },
    ];
}