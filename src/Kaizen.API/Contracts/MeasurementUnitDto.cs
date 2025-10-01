namespace Kaizen.API.Contracts;

public record MeasurementUnitDto
{
    public required string Code { get; init; }
    public required string Name { get; init; }
}