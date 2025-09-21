using Kaizen.API.Contracts.Exercises;

namespace Kaizen.API.Models;

public static class MuscleGroupExtensions
{
    public static MuscleGroupDto ToMuscleGroupDto(this MuscleGroup muscleGroup)
    {
        return new MuscleGroupDto
        {
            Code = muscleGroup.Code,
            Name = muscleGroup.Name,
        };
    }

    public static IEnumerable<MuscleGroupDto> ToMuscleGroupDtos(this IEnumerable<MuscleGroup> muscleGroups)
    {
        return muscleGroups.Select(ToMuscleGroupDto);
    }
}