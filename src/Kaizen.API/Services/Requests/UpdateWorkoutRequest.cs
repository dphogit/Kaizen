namespace Kaizen.API.Services.Requests;

public record UpdateWorkoutRequest
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required DateTimeOffset PerformedAt { get; init; }
    public ICollection<Set> Sets { get; init; } = [];
    public required string UserId { get; init; }

    public record Set
    {
        public required int ExerciseId { get; init; }
        public required int Repetitions { get; init; }
        public required decimal Quantity { get; init; }
        public required string MeasurementUnitCode { get; init; }
    }
}