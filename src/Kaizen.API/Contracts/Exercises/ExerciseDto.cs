namespace Kaizen.API.Contracts.Exercises;

public record ExerciseDto : TimestampedDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required IList<MuscleGroupDto> MuscleGroups { get; init; }
}

public static class ExerciseDtoExtensions
{
    public static CreateExerciseDto ToCreateDto(this ExerciseDto exerciseDto)
    {
        return new CreateExerciseDto
        {
            Name = exerciseDto.Name,
            MuscleGroupCodes = exerciseDto.MuscleGroups.Select(m => m.Code).ToList(),
        };
    }
}