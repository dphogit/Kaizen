using FluentResults;
using Kaizen.API.Data;
using Kaizen.API.Models;
using Kaizen.API.Services.Results;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Services;

public class ExerciseService(KaizenDbContext dbContext) : IExerciseService
{
    public async Task<Result<Exercise>> CreateExerciseAsync(string name, IEnumerable<string> muscleGroupCodes)
    {
        var errors = new List<IError>();
        
        if (await dbContext.Exercises.AnyAsync(e => e.Name.ToLower() == name.ToLower()))
        {
            errors.Add(new Error($"The exercise '{name}' already exists."));
        }
        
        var muscleGroupsValidationResult = await ValidateMuscleGroups(muscleGroupCodes);

        if (muscleGroupsValidationResult.IsFailed)
        {
            errors.AddRange(muscleGroupsValidationResult.Errors);
        }

        if (errors.Count > 0)
        {
            return Result.Fail(errors);
        }

        var exercise = new Exercise
        {
            Name = name,
            MuscleGroups = muscleGroupsValidationResult.Value
        };

        await dbContext.Exercises.AddAsync(exercise);
        await dbContext.SaveChangesAsync();
        
        return Result.Ok(exercise);
    }

    public async Task<Result<Exercise>> UpdateExerciseAsync(int id, string name, IEnumerable<string> muscleGroupCodes)
    {
        var exercise = await GetExerciseAsync(id);

        if (exercise is null)
        {
            return Result.Fail(new NotFoundError());
        }
        
        var muscleGroupsValidationResult = await ValidateMuscleGroups(muscleGroupCodes);

        if (muscleGroupsValidationResult.IsFailed)
        {
            return Result.Fail(muscleGroupsValidationResult.Errors);
        }
        
        exercise.Name = name;
        
        // Replace the existing join entities with the new values
        exercise.MuscleGroups.Clear();
        foreach (var muscleGroup in muscleGroupsValidationResult.Value)
        {
            exercise.MuscleGroups.Add(muscleGroup);
        }
        
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

    public async Task<IList<Exercise>> GetExercisesAsync()
    {
        var exercises = await dbContext.Exercises
            .Include(e => e.MuscleGroups)
            .ToListAsync();
        
        return exercises;
    }

    private async Task<Result<List<MuscleGroup>>> ValidateMuscleGroups(IEnumerable<string> muscleGroupCodes)
    {
        var requestCodes = muscleGroupCodes.Select(c => c.ToLower()).ToList();

        if (requestCodes.Count == 0)
        {
            return Result.Fail("At least one muscle group code must be provided.");
        }

        // Validate muscle group codes are all valid.
        var validMuscleGroups = await dbContext.MuscleGroups.Where(mg => requestCodes.Contains(mg.Code)).ToListAsync();

        if (validMuscleGroups.Count != requestCodes.Count)
        {
            var dbCodes = dbContext.MuscleGroups.Select(mg => mg.Code);
            var invalidCodes = requestCodes.Except(dbCodes);
            var invalidCodeMessage = $"Invalid muscle group codes: {string.Join(", ", invalidCodes)}";
            return Result.Fail(invalidCodeMessage);
        }
        
        return Result.Ok(validMuscleGroups);
    }
}