using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Kaizen.API.Contracts.Exercises;
using Kaizen.API.FunctionalTests.Fakes;
using Kaizen.API.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.FunctionalTests.Exercises;

[Collection(nameof(ApiTestCollection))]
public class CreateExerciseTests : BaseExerciseTests
{
    public CreateExerciseTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateExercise_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/exercises", ExerciseFakes.UpsertBenchPress);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateExercise_NormalUser_ReturnsForbidden()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();

        // Act
        var response = await client.PostAsJsonAsync("/exercises", ExerciseFakes.UpsertBenchPress);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CreateExercise_Admin_SavesNewExercise()
    {
        // Arrange
        var client = Fixture.Factory.CreateAdminClient();
        var benchPressRequest = ExerciseFakes.UpsertBenchPress;

        // Act
        var response = await client.PostAsJsonAsync("/exercises", benchPressRequest);

        // Assert - POST response header/body and can GET from location (how a client would interact with the API)
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var exercise = await response.Content.ReadFromJsonAsync<ExerciseDto>();
        Assert.NotNull(exercise);
        Assert.Equal($"/exercises/{exercise.Id}", response.Headers.Location?.ToString());
        ExerciseAssertions.Equal(exercise, benchPressRequest);

        response = await client.GetAsync($"/exercises/{exercise.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        exercise = await response.Content.ReadFromJsonAsync<ExerciseDto>();
        Assert.NotNull(exercise);
        ExerciseAssertions.Equal(exercise, benchPressRequest);
    }

    [Fact]
    public async Task CreateExercise_InvalidDetails_ReturnsBadRequest()
    {
        // Arrange
        var client = Fixture.Factory.CreateAdminClient();

        var request = new UpsertExerciseDto
        {
            Name = "Invalid Exercise",
            MuscleGroupCodes = ["invalid_muscle"]
        };

        // Act
        var response = await client.PostAsJsonAsync("/exercises", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertResponseHasProblemDetailsErrors(response);
    }
}