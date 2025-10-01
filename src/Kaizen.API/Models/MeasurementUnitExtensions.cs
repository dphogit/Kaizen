using Kaizen.API.Contracts;

namespace Kaizen.API.Models;

public static class MeasurementUnitExtensions
{
    public static MeasurementUnitDto ToMeasurementUnitDto(this MeasurementUnit mu)
    {
        return new MeasurementUnitDto { Code = mu.Code, Name = mu.Name };
    }

    public static IEnumerable<MeasurementUnitDto> ToMeasurementUnitDtos(this IEnumerable<MeasurementUnit> units)
    {
        return units.Select(ToMeasurementUnitDto);
    }
}