using System.Net;
using Kaizen.API.Contracts.Workouts;
using Kaizen.API.FunctionalTests.Fakes;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Workouts;

[Collection(nameof(ApiTestCollection))]
public class DeleteWorkoutTests : BaseApiTests
{
    public DeleteWorkoutTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task DeleteWorkout_IsOwner_ReturnsNoContentAndDeletesResource()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();

        var exercise = await CreateExercise(ExerciseFakes.UpsertBenchPress);

        var workoutReq = new UpsertWorkoutDto
        {
            Name = "Test Workout",
            PerformedAt = DateTimeOffset.UtcNow,
            Sets = [new() { ExerciseId = exercise.Id, Repetitions = 10, Quantity = 20, MeasurementUnitCode = "kg"}]
        };

        var workout = await CreateWorkout(workoutReq);
        
        // Act
        var response = await client.DeleteAsync($"/workouts/{workout.Id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        response = await client.GetAsync($"/workouts/{workout.Id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteWorkout_NotExists_ReturnsNotFound()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();

        // Act
        var response = await client.DeleteAsync($"/workouts/1");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteWorkout_NotOwner_ReturnsNotFound()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();

        var exercise = await CreateExercise(ExerciseFakes.UpsertBenchPress);

        var workoutReq = new UpsertWorkoutDto
        {
            Name = "Test Workout",
            PerformedAt = DateTimeOffset.UtcNow,
            Sets = [new() { ExerciseId = exercise.Id, Repetitions = 10, Quantity = 20, MeasurementUnitCode = "kg"}]
        };

        var workout = await CreateWorkout(workoutReq, "AnotherUser");
        
        // Act
        var response = await client.DeleteAsync($"/workouts/{workout.Id}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteWorkout_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"/workouts/1");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}