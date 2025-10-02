using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts.Workouts;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Workouts;

[Collection(nameof(ApiTestCollection))]
public class UpdateWorkoutTests : UpsertWorkoutTests
{
    public UpdateWorkoutTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task UpdateWorkout_SavesNewDetails()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        var pushDayRequest = await CreatePushDayRequest();
        var pushDayWorkout = await CreateWorkout(pushDayRequest);

        var modifiedPushDayRequest = pushDayRequest with
        {
            Name = "Push Day (Updated)",
            PerformedAt = DateTimeOffset.UtcNow,
            Sets = [pushDayRequest.Sets[0]]
        };
        
        // Act
        var response = await client.PutAsJsonAsync($"/workouts/{pushDayWorkout.Id}", modifiedPushDayRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var workout = await response.Content.ReadFromJsonAsync<WorkoutDto>();
        Assert.NotNull(workout);
        
        WorkoutAssertions.AssertEqual(modifiedPushDayRequest, workout);
    }
    
    [Fact]
    public async Task UpdateWorkout_NotCreator_ReturnsNotFound()
    {
        // Arrange
        var pushDayRequest = await CreatePushDayRequest();
        var pushDayWorkout = await CreateWorkout(pushDayRequest, "User1");

        var client = Fixture.Factory.CreateAuthenticatedClient("User2");
        
        // Act
        var response = await client.PutAsJsonAsync($"/workouts/{pushDayWorkout.Id}", pushDayRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]
    public async Task UpdateWorkout_NoWorkoutWithId_ReturnsNotFound()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        var pushDayRequest = await CreatePushDayRequest();
        
        // Act
        var response = await client.PutAsJsonAsync("/workouts/1", pushDayRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateWorkout_InvalidDetails_ReturnsBadRequest()
    {
        // Arrange
        var pushDayRequest = await CreatePushDayRequest();
        var pushDayWorkout = await CreateWorkout(pushDayRequest);
        
        pushDayRequest.Sets.Clear(); // Workouts must have at least one set
        
        var client = Fixture.Factory.CreateAuthenticatedClient();

        // Act
        var response = await client.PutAsJsonAsync($"/workouts/{pushDayWorkout.Id}", pushDayRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertResponseHasProblemDetailsErrors(response);
    }

    [Fact]
    public async Task UpdateWorkout_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();
        
        // Act
        var response = await client.PutAsJsonAsync("/workouts/1", new { });
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}