namespace Kaizen.API.Contracts.Exercises;

public record MuscleGroupDto
{
    public required string Code { get; init; }
    public required string Name { get; init; }
}