namespace Kaizen.API.Models;

public sealed class Exercise : TimestampedEntity
{
    public int Id { get; set; } 
    public required string Name { get; set; }
    
    public ICollection<MuscleGroup> MuscleGroups { get; init; } = [];
}