using Kaizen.API.Contracts.Workouts;

namespace Kaizen.API.FunctionalTests.Workouts;

public static class WorkoutAssertions
{
    public static void AssertEqual(WorkoutDto expected, WorkoutDto actual)
    {
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.PerformedAt, actual.PerformedAt);
        
        Assert.Equal(expected.Sets.Count, actual.Sets.Count);
        Assert.All(expected.Sets, set => Assert.Contains(set, actual.Sets));
    }
    
    public static void AssertEqual(RecordWorkoutDto r, WorkoutDto w)
    {
        Assert.Equal(r.Name, w.Name);
        Assert.Equal(r.PerformedAt, w.PerformedAt);
        
        Assert.Equal(r.Sets.Count, w.Sets.Count);
        Assert.All(r.Sets, set => AssertContains(set, w));
    }

    private static void AssertContains(RecordWorkoutDto.Set r, WorkoutDto w)
    {
        var foundSet = w.Sets.FirstOrDefault(set => AreEqual(r, set));
        Assert.NotNull(foundSet);
    }

    private static bool AreEqual(RecordWorkoutDto.Set r, WorkoutDto.Set s)
    {
        return r.ExerciseId == s.ExerciseId &&
               r.Repetitions == s.Repetitions &&
               r.Quantity == s.Quantity  &&
               r.MeasurementUnitCode == s.MeasurementUnitCode;
    }
}