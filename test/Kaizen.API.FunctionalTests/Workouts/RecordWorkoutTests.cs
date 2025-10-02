using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts.Workouts;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Workouts;

[Collection(nameof(ApiTestCollection))]
public class RecordWorkoutTests : UpsertWorkoutTests
{
    public RecordWorkoutTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task RecordWorkout_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();
        
        var pushDayRequest = await CreatePushDayRequest();
        
        // Act
        var response = await client.PostAsJsonAsync("/workouts", pushDayRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RecordWorkout_ValidAndAuthenticated_SavesWorkout()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();

        var pushDayRequest = await CreatePushDayRequest();
        
        // Act
        var response = await client.PostAsJsonAsync("/workouts", pushDayRequest);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var workout = await response.Content.ReadFromJsonAsync<WorkoutDto>();
        Assert.NotNull(workout);
        Assert.Equal($"/workouts/{workout.Id}", response.Headers.Location?.ToString());
        WorkoutAssertions.AssertEqual(pushDayRequest, workout);

        response = await client.GetAsync($"/workouts/{workout.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        workout = await response.Content.ReadFromJsonAsync<WorkoutDto>();
        Assert.NotNull(workout);
        WorkoutAssertions.AssertEqual(pushDayRequest, workout);
    }

    [Fact]
    public async Task RecordWorkout_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        var pushDayRequest = await CreatePushDayRequest();
        pushDayRequest.Sets.Clear(); // Workouts must have at least one set
        
        // Act
        var response = await client.PostAsJsonAsync("/workouts", pushDayRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertResponseHasProblemDetailsErrors(response);
    }
}