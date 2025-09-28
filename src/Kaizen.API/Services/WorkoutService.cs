using FluentResults;
using Kaizen.API.Data;
using Kaizen.API.Models;
using Kaizen.API.Services.Requests;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Services;

public class WorkoutService(KaizenDbContext dbContext) : IWorkoutService
{
    public async Task<Result<Workout>> CreateWorkoutAsync(CreateWorkoutRequest request)
    {
        var errors = new List<IError>();

        if (!await UserExistsAsync(request.UserId))
        {
            errors.Add(new Error($"User id {request.UserId} not found."));
        }

        var workoutSetsValidationResult = await ValidateWorkoutSetsAsync(request.Sets);

        if (workoutSetsValidationResult.IsFailed)
        {
            errors.AddRange(workoutSetsValidationResult.Errors);
        }

        if (errors.Count > 0)
        {
            return Result.Fail(errors);
        }

        var workout = new Workout
        {
            Name = request.Name,
            PerformedAt = request.PerformedAt,
            UserId = request.UserId,
        };

        foreach (var set in request.Sets)
        {
            workout.Sets.Add(new WorkoutSet
            {
                ExerciseId = set.ExerciseId,
                Repetitions = set.Repetitions,
                Quantity = set.Quantity,
                MeasurementUnitCode = set.MeasurementUnitCode
            });
        }

        await dbContext.Workouts.AddAsync(workout);
        await dbContext.SaveChangesAsync();

        return Result.Ok(workout);
    }

    public async Task<IList<Workout>> GetWorkoutsAsync(GetWorkoutsFilters filters)
    {
        return await dbContext.Workouts
            .Where(w => w.UserId == filters.UserId)
            .OrderByDescending(w => w.PerformedAt)
            .Include(w => w.Sets)
            .ThenInclude(s => s.Exercise)
            .ToListAsync();
    }

    public async Task<Workout?> GetWorkoutAsync(long id)
    {
        var workout = await dbContext.Workouts
            .Include(w => w.Sets)
            .ThenInclude(s => s.Exercise)
            .FirstOrDefaultAsync(w => w.Id == id);

        return workout;
    }

    private Task<bool> UserExistsAsync(string userId)
    {
        return dbContext.Users.AnyAsync(u => u.Id == userId);
    }

    private async Task<Result<List<WorkoutSet>>> ValidateWorkoutSetsAsync(ICollection<CreateWorkoutRequest.Set> sets)
    {
        if (sets.Count == 0)
        {
            return Result.Fail("A workout must have at least one recorded set.");
        }
        
        List<IError> errors = [];

        var requestedExerciseIds = sets.Select(s => s.ExerciseId).Distinct();
        var requestMeasurementUnitCodes = sets.Select(s => s.MeasurementUnitCode).Distinct();

        var validExercises = await dbContext.Exercises
            .Where(e => requestedExerciseIds.Contains(e.Id))
            .ToListAsync();

        var validMeasurementUnits = await dbContext.MeasurementUnits
            .Where(mu => requestMeasurementUnitCodes.Contains(mu.Code))
            .ToListAsync();

        var invalidExerciseIds = validExercises.ExceptBy(requestedExerciseIds, ex => ex.Id).ToList();
        var invalidMeasurementUnitCodes = validMeasurementUnits
            .ExceptBy(requestMeasurementUnitCodes, mu => mu.Code)
            .ToList();

        if (invalidExerciseIds.Count > 0)
        {
            errors.Add(new Error($"Invalid ExerciseIds: {string.Join(", ", invalidExerciseIds)}"));
        }

        if (invalidMeasurementUnitCodes.Count > 0)
        {
            errors.Add(new Error($"Invalid MeasurementUnitCodes: {string.Join(", ", invalidMeasurementUnitCodes)}"));
        }

        if (errors.Count > 0)
        {
            return Result.Fail(errors);
        }

        var workoutSets = sets.Select(s => new WorkoutSet
        {
            Exercise = validExercises.Single(e => e.Id == s.ExerciseId),
            ExerciseId = s.ExerciseId,
            Repetitions = s.Repetitions,
            Quantity = s.Quantity,
            MeasurementUnit = validMeasurementUnits.Single(e => e.Code == s.MeasurementUnitCode),
            MeasurementUnitCode = s.MeasurementUnitCode
        });
        
        return Result.Ok(workoutSets.ToList());
    }
}