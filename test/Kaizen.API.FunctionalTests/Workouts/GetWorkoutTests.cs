using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts.Workouts;
using Kaizen.API.FunctionalTests.Fakes;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Workouts;

[Collection(nameof(ApiTestCollection))]
public class GetWorkoutTests : BaseApiTests
{
    public GetWorkoutTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetWorkoutById_NotExist_ReturnsNotFound()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();

        // Act
        var response = await client.GetAsync("/workouts/1");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetWorkoutById_AuthenticatedAndOwn_ReturnsWorkout()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        var benchPress = await CreateExercise(ExerciseFakes.UpsertBenchPress);

        var recordWorkoutDto = new UpsertWorkoutDto
        {
            Name = "Bench Press",
            PerformedAt = DateTimeOffset.Now,
            Sets =
            [
                new UpsertWorkoutDto.Set
                    { ExerciseId = benchPress.Id, Repetitions = 10, Quantity = 20, MeasurementUnitCode = "kg" }
            ]
        };
        
        var workoutDto = await CreateWorkout(recordWorkoutDto);
        
        // Act
        var response = await client.GetAsync($"/workouts/{workoutDto.Id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var retrievedDto = await response.Content.ReadFromJsonAsync<WorkoutDto>();
        Assert.NotNull(retrievedDto);
        WorkoutAssertions.AssertEqual(workoutDto, retrievedDto);
    }

    [Fact]
    public async Task GetWorkoutById_NotOwn_ReturnsNotFound()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient("User1");

        var benchPress = await CreateExercise(ExerciseFakes.UpsertBenchPress);

        var recordWorkoutDto = new UpsertWorkoutDto
        {
            Name = "Bench Press",
            PerformedAt = DateTimeOffset.Now,
            Sets =
            [
                new UpsertWorkoutDto.Set
                    { ExerciseId = benchPress.Id, Repetitions = 10, Quantity = 20, MeasurementUnitCode = "kg" }
            ]
        };

        var workout = await CreateWorkout(recordWorkoutDto, "User2");

        // Act
        var response = await client.GetAsync($"/workouts/{workout.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetWorkoutById_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();

        // Act
        var response = await client.GetAsync("/workouts/1");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}