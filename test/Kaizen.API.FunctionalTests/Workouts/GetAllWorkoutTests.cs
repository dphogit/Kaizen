using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts.Workouts;
using Kaizen.API.FunctionalTests.Fakes;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Workouts;

[Collection(nameof(ApiTestCollection))]
public class GetAllWorkoutTests : BaseApiTests
{
    public GetAllWorkoutTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetWorkouts_Authenticated_ReturnsOwnWorkoutsInMostRecentOrder()
    {
        // Arrange
        var exercise1 = await CreateExercise(ExerciseFakes.UpsertBenchPress);
        var exercise2 = await CreateExercise(ExerciseFakes.UpsertShoulderPress);
        var exercise3 = await CreateExercise(ExerciseFakes.UpsertRomanianDeadlift);

        var req1 = new RecordWorkoutDto
        {
            Name = "Workout 1",
            PerformedAt = new DateTime(2025, 08, 28),
            Sets =
            [
                new RecordWorkoutDto.Set
                    { ExerciseId = exercise1.Id, Repetitions = 10, Quantity = 20, MeasurementUnitCode = "kg" }
            ]
        };

        var req2 = new RecordWorkoutDto
        {
            Name = "Workout 2",
            PerformedAt = new DateTime(2025, 08, 29),
            Sets =
            [
                new RecordWorkoutDto.Set
                    { ExerciseId = exercise2.Id, Repetitions = 10, Quantity = 5, MeasurementUnitCode = "lvl" }
            ]
        };

        var req3 = new RecordWorkoutDto
        {
            Name = "Workout 3 - Another User",
            PerformedAt = new DateTime(2025, 08, 30),
            Sets =
            [
                new RecordWorkoutDto.Set
                    { ExerciseId = exercise3.Id, Repetitions = 6, Quantity = 25, MeasurementUnitCode = "kg" }
            ]
        };
        
        var workout1 = await CreateWorkout(req1);
        var workout2 = await CreateWorkout(req2);
        await CreateWorkout(req3, "AnotherUser");
        
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        // Act
        var response = await client.GetAsync("/workouts");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var returnedWorkouts = await response.Content.ReadFromJsonAsync<List<WorkoutDto>>();
        Assert.NotNull(returnedWorkouts);
        
        Assert.Equal(2, returnedWorkouts.Count);
        WorkoutAssertions.AssertEqual(workout2, returnedWorkouts[0]);
        WorkoutAssertions.AssertEqual(workout1, returnedWorkouts[1]);
    }

    [Fact]
    public async Task GetWorkouts_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/workouts");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}