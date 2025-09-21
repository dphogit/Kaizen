namespace Kaizen.API.Contracts;

public record TimestampedDto
{
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
}