namespace Kaizen.API.Contracts.Workouts;

public record WorkoutDto : TimestampedDto
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required DateTimeOffset PerformedAt { get; init; }
    public required ICollection<Set> Sets { get; init; }
    
    public record Set 
    {
        public long Id { get; init; }
        
        public required int ExerciseId { get; init; }
        public required string ExerciseName { get; init; }
        
        public required int Repetitions { get; init; }
        
        public required decimal Quantity { get; init; }
        
        public required string MeasurementUnitCode { get; init; }
    }
}