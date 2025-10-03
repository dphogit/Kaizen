using Kaizen.API.Contracts.Workouts;

namespace Kaizen.API.Models;

public static class WorkoutExtensions
{
    public static WorkoutDto ToWorkoutDto(this Workout workout)
    {
        return new WorkoutDto
        {
            Id = workout.Id,
            Name = workout.Name,
            PerformedAt = workout.PerformedAt,
            Sets = workout.Sets.ToWorkoutSetDtos().ToList(),
            CreatedAt = workout.CreatedAt,
            UpdatedAt = workout.UpdatedAt,
        };
    }

    public static IEnumerable<WorkoutDto> ToWorkoutDtos(this IEnumerable<Workout> workouts)
    {
        return workouts.Select(ToWorkoutDto);
    }

    public static WorkoutDto.Set ToWorkoutSetDto(this WorkoutSet workoutSet)
    {
        return new WorkoutDto.Set()
        {
            Id = workoutSet.Id,
            Quantity = workoutSet.Quantity,
            Repetitions = workoutSet.Repetitions,
            ExerciseId = workoutSet.ExerciseId,
            ExerciseName = workoutSet.Exercise.Name,
            MeasurementUnitCode = workoutSet.MeasurementUnitCode
        };
    }

    public static IEnumerable<WorkoutDto.Set> ToWorkoutSetDtos(this IEnumerable<WorkoutSet> workoutSets)
    {
        return workoutSets.Select(ToWorkoutSetDto);
    }
}