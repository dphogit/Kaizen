namespace Kaizen.API.Models;

public class Workout : TimestampedEntity
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public DateTimeOffset PerformedAt { get; set; }

    public ICollection<WorkoutSet> Sets { get; } = [];

    public string UserId { get; set; } = null!;
    public KaizenUser User { get; set; } = null!;
}