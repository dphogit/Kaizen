namespace Kaizen.API.Services.Requests;

/// <summary>
/// The request object for workout service to create a workout.
/// </summary>
public record CreateWorkoutRequest
{
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
};