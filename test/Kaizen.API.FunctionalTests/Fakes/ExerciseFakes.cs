using Kaizen.API.Contracts.Exercises;

namespace Kaizen.API.FunctionalTests.Fakes;

public static class ExerciseFakes
{
    public static UpsertExerciseDto UpsertBenchPress => new()
    {
        Name = "Bench Press",
        MuscleGroupCodes = ["chest", "shoulders", "triceps"]
    };

    public static UpsertExerciseDto UpsertRomanianDeadlift => new()
    {
        Name = "Romanian Deadlift",
        MuscleGroupCodes = ["hamstrings", "glutes", "lower_back"]
    };
    
    public static UpsertExerciseDto UpdateShoulderPress => new()
    {
        Name = "Shoulder Press",
        MuscleGroupCodes = ["shoulders", "triceps", "traps"]
    };
}