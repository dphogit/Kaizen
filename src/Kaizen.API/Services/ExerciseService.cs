using FluentResults;
using Kaizen.API.Data;
using Kaizen.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Services;

public class ExerciseService(KaizenDbContext dbContext) : IExerciseService
{
    public async Task<Result<Exercise>> CreateExerciseAsync(string name, IEnumerable<string> muscleGroupCodes)
    {
        var errors = new List<string>();
        
        var requestCodes = muscleGroupCodes.Select(c => c.ToLower()).ToList();

        List<MuscleGroup> validMuscleGroups = [];
        
        if (requestCodes.Count == 0)
        {
            errors.Add("At least one muscle group code must be provided.");
        }
        else
        {
            // Validate muscle group codes are valid.
            validMuscleGroups = await dbContext.MuscleGroups.Where(mg => requestCodes.Contains(mg.Code)).ToListAsync();

            if (validMuscleGroups.Count != requestCodes.Count)
            {
                var dbCodes = dbContext.MuscleGroups.Select(mg => mg.Code);
                var invalidCodes = requestCodes.Except(dbCodes);
                var invalidCodeMessage = $"Invalid muscle group codes: {string.Join(", ", invalidCodes)}";
                errors.Add(invalidCodeMessage);
            }
        }

        if (await dbContext.Exercises.AnyAsync(e => e.Name.ToLower() == name.ToLower()))
        {
            errors.Add($"The exercise '{name}' already exists.");
        }

        if (errors.Count > 0)
        {
            return Result.Fail(errors);
        }

        var exercise = new Exercise
        {
            Name = name,
            MuscleGroups = validMuscleGroups
        };

        await dbContext.Exercises.AddAsync(exercise);
        await dbContext.SaveChangesAsync();

        return Result.Ok(exercise);
    }

    public async Task<Exercise?> GetExerciseAsync(int id)
    {
        var exercise = await dbContext.Exercises
            .Include(e => e.MuscleGroups)
            .FirstOrDefaultAsync(e => e.Id == id);
        
        return exercise;
    }
}