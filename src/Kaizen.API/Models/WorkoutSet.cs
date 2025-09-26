namespace Kaizen.API.Models;

public class WorkoutSet
{
    public long Id { get; set; }
    
    public long WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;
    
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    
    public int Repetitions { get; set; }
    
    public decimal Quantity { get; set; }

    public string MeasurementUnitCode { get; set; } = null!;
    public MeasurementUnit MeasurementUnit { get; set; } = null!;
}