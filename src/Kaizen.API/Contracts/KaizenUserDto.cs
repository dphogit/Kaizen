namespace Kaizen.API.Contracts;

public record KaizenUserDto
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string[] Roles { get; init; }
}