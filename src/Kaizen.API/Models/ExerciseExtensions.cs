using Kaizen.API.Contracts.Exercises;

namespace Kaizen.API.Models;

public static class ExerciseExtensions
{
    public static ExerciseDto ToExerciseDto(this Exercise exercise)
    {
        return new ExerciseDto
        {
            Id = exercise.Id,
            Name = exercise.Name,
            MuscleGroups = exercise.MuscleGroups.ToMuscleGroupDtos().ToList(),
            CreatedAt = exercise.CreatedAt,
            UpdatedAt = exercise.UpdatedAt,
        };
    }
}