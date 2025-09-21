namespace Kaizen.API.Contracts.Exercises;

public record ExerciseDto : TimestampedDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required IList<MuscleGroupDto> MuscleGroups { get; init; }
}