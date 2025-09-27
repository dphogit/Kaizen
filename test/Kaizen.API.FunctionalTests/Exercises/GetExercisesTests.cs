using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts.Exercises;
using Kaizen.API.FunctionalTests.Fakes;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Exercises;

[Collection(nameof(ApiTestCollection))]
public class GetExercisesTests : BaseApiTests
{
    public GetExercisesTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetExercises_Authenticated_ReturnsAllExercises()
    {
        // Arrange
        var benchPress = await CreateExercise(ExerciseFakes.UpsertBenchPress);
        var romanianDeadlift = await CreateExercise(ExerciseFakes.UpsertRomanianDeadlift);
        
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        // Act
        var response = await client.GetAsync("/exercises");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var exercises = await response.Content.ReadFromJsonAsync<ExerciseDto[]>();
        Assert.NotNull(exercises);
        
        Assert.Equal(2, exercises.Length);

        var benchPressDto = Assert.Single(exercises, e => e.Name == benchPress.Name);
        ExerciseAssertions.Equal(benchPressDto, benchPress);
        
        var rdlDto = Assert.Single(exercises, e => e.Name == romanianDeadlift.Name);
        ExerciseAssertions.Equal(rdlDto, romanianDeadlift);
    }

    [Fact]
    public async Task GetExercises_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/exercises");
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetExerciseById_Existing_ReturnsExercise()
    {
        // Arrange
        var benchPress = await CreateExercise(ExerciseFakes.UpsertBenchPress);
        
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        // Act
        var response = await client.GetAsync($"/exercises/{benchPress.Id}");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var exercise = await response.Content.ReadFromJsonAsync<ExerciseDto>();
        Assert.NotNull(exercise);
        
        ExerciseAssertions.Equal(exercise, benchPress);
    }

    [Fact]
    public async Task GetExerciseById_NotExist_ReturnsNotFound()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        // Act
        var response = await client.GetAsync($"/exercises/1");
        
        // Arrange
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetExerciseById_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = Fixture.Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/exercises/1");
        
        // Arrange
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}