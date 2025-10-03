namespace Kaizen.API.Contracts.Workouts;

public record UpsertWorkoutDto
{
    public required string Name { get; init; }
    public required DateTimeOffset PerformedAt { get; init; }
    public List<Set> Sets { get; init; } = [];

    public record Set
    {
        public required int ExerciseId { get; init; }
        public required int Repetitions { get; init; }
        public required decimal Quantity { get; init; }
        public required string MeasurementUnitCode { get; init; }
    }
}
