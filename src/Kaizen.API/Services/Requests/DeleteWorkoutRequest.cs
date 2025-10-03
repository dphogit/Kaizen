namespace Kaizen.API.Services.Requests;

public record DeleteWorkoutRequest
{
    public required long WorkoutId { get; init; }
    public required string UserId { get; init; }
}