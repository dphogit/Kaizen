using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts.Exercises;
using Kaizen.API.FunctionalTests.Fakes;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Exercises;

[Collection(nameof(ApiTestCollection))]
public class UpdateExerciseTests : BaseApiTests
{
    public UpdateExerciseTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task UpdateExercise_Admin_SavesNewExerciseDetails()
    {
        // Arrange
        var benchPress = await CreateExercise(ExerciseFakes.UpsertBenchPress);

        var client = Fixture.Factory.CreateAdminClient();

        var updatedBenchPress = new UpsertExerciseDto
        {
            Name = "Bench Press II",
            MuscleGroupCodes = ["chest", "shoulders", "triceps", "traps"]
        };
        
        // Act
        var response = await client.PutAsJsonAsync($"/exercises/{benchPress.Id}", updatedBenchPress);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var savedExercise = await response.Content.ReadFromJsonAsync<ExerciseDto>();
        Assert.NotNull(savedExercise);
        ExerciseAssertions.Equal(savedExercise, updatedBenchPress);

        var fetchedExercise = await client.GetFromJsonAsync<ExerciseDto>($"exercises/{benchPress.Id}");
        Assert.NotNull(fetchedExercise);
        ExerciseAssertions.Equal(fetchedExercise, updatedBenchPress);
    }

    [Fact]
    public async Task UpdateExercise_NoExerciseWithId_ReturnsNotFound()
    {
        // Arrange
        var client = Fixture.Factory.CreateAdminClient();
        
        // Act
        var response = await client.PutAsJsonAsync("/exercises/1", ExerciseFakes.UpsertShoulderPress);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateExercise_InvalidDetails_ReturnsBadRequest()
    {
        // Arrange
        var benchPress = await CreateExercise(ExerciseFakes.UpsertBenchPress);
        
        var client = Fixture.Factory.CreateAdminClient();

        var badUpdate = new UpsertExerciseDto
        {
            Name = "Bad Bench Press",
            MuscleGroupCodes = []   // No codes given
        };
        
        // Act
        var response = await client.PutAsJsonAsync($"/exercises/{benchPress.Id}", badUpdate);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await AssertResponseHasProblemDetailsErrors(response);
    }

    [Fact]
    public async Task UpdateExercise_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();
        
        // Act
        var response = await client.PutAsJsonAsync("/exercises/1", ExerciseFakes.UpsertShoulderPress);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
        
    [Fact]
    public async Task UpdateExercise_NormalUser_ReturnsForbidden()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        // Act
        var response = await client.PutAsJsonAsync("/exercises/1", ExerciseFakes.UpsertShoulderPress);
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}