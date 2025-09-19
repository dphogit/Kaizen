namespace Kaizen.API.Models;

/// <summary>The join entity for <see cref="Exercise"/> and <see cref="MuscleGroup"/>.</summary>
public class ExerciseMuscleGroup
{
    public required int ExerciseId { get; set; }
    public required string MuscleGroupCode { get; set; }
}