namespace Kaizen.API.Contracts.Exercises;

public record CreateExerciseDto
{
    public required string Name { get; init; }
    public required IList<string> MuscleGroupCodes  { get; init; }
}