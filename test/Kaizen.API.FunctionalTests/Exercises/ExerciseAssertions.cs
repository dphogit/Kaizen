using Kaizen.API.Contracts.Exercises;

namespace Kaizen.API.FunctionalTests.Exercises;

public static class ExerciseAssertions
{
    public static void Equal(ExerciseDto expected, ExerciseDto actual)
    {
        Equal(expected, actual.ToCreateDto());
        
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.CreatedAt, actual.CreatedAt);
        Assert.Equal(expected.UpdatedAt, actual.UpdatedAt);
    }

    public static void Equal(ExerciseDto expected, CreateExerciseDto actual)
    {
        Assert.Equal(expected.Name, actual.Name);
        
        Assert.Equal(expected.MuscleGroups.Count, actual.MuscleGroupCodes.Count);
        Assert.All(expected.MuscleGroups, mgDto => Assert.Contains(mgDto.Code, actual.MuscleGroupCodes));
    }
}