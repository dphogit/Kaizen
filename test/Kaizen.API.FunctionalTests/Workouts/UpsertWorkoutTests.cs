using Kaizen.API.Contracts.Workouts;
using Kaizen.API.FunctionalTests.Fakes;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Workouts;

public abstract class UpsertWorkoutTests : BaseApiTests
{
    public UpsertWorkoutTests(ApiTestFixture fixture) : base(fixture)
    {
    }
    
    protected async Task<UpsertWorkoutDto> CreatePushDayRequest()
    {
        var benchPress = await CreateExercise(ExerciseFakes.UpsertBenchPress);
        var shoulderPress = await CreateExercise(ExerciseFakes.UpsertShoulderPress);

        var recordWorkoutDto = new UpsertWorkoutDto
        {
            Name = "Push Day",
            PerformedAt = DateTimeOffset.UtcNow,
            Sets =
            [
                new UpsertWorkoutDto.Set
                    { ExerciseId = benchPress.Id, Repetitions = 8, Quantity = 20, MeasurementUnitCode = "kg" },
                new UpsertWorkoutDto.Set
                    { ExerciseId = benchPress.Id, Repetitions = 8, Quantity = 20, MeasurementUnitCode = "kg" },

                new UpsertWorkoutDto.Set
                    { ExerciseId = shoulderPress.Id, Repetitions = 10, Quantity = 5, MeasurementUnitCode = "lvl" },
                new UpsertWorkoutDto.Set
                    { ExerciseId = shoulderPress.Id, Repetitions = 10, Quantity = 5, MeasurementUnitCode = "lvl" },
            ]
        };

        return recordWorkoutDto;
    }
}