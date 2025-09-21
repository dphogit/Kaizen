using Kaizen.API.Contracts.Exercises;

namespace Kaizen.API.FunctionalTests.Fakes;

public static class FakeCreateExerciseDto
{
    public static CreateExerciseDto BenchPress => new()
    {
        Name = "Bench Press",
        MuscleGroupCodes = ["chest", "shoulders", "triceps"]
    };

    public static CreateExerciseDto RomanianDeadlift => new()
    {
        Name = "Romanian Deadlift",
        MuscleGroupCodes = ["hamstrings", "glutes", "lower_back"]
    };
}