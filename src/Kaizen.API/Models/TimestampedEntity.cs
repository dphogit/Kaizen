namespace Kaizen.API.Models;

public abstract class TimestampedEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}