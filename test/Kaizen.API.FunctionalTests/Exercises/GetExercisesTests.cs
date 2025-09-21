using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts.Exercises;
using Kaizen.API.FunctionalTests.Fakes;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Exercises;

[Collection(nameof(ApiTestCollection))]
public class GetExercisesTests(ApiTestFixture fixture) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await fixture.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<ExerciseDto> CreateExercise(CreateExerciseDto request)
    {
        using var client = fixture.Factory.CreateAdminClient();

        var response = await client.PostAsJsonAsync("/exercises", request);
        response.EnsureSuccessStatusCode();
        
        var exercise = await response.Content.ReadFromJsonAsync<ExerciseDto>();
        Assert.NotNull(exercise);
        
        return exercise;
    }

    [Fact]
    public async Task GetExercises_Authenticated_ReturnsAllExercises()
    {
        // Arrange
        var benchPress = await CreateExercise(FakeCreateExerciseDto.BenchPress);
        var romanianDeadlift = await CreateExercise(FakeCreateExerciseDto.RomanianDeadlift);
        
        var client = fixture.Factory.CreateAuthenticatedClient();
        
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
        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/exercises");
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetExerciseById_Existing_ReturnsExercise()
    {
        // Arrange
        var benchPress = await CreateExercise(FakeCreateExerciseDto.BenchPress);
        
        var client = fixture.Factory.CreateAuthenticatedClient();
        
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
        var client = fixture.Factory.CreateAuthenticatedClient();
        
        // Act
        var response = await client.GetAsync($"/exercises/1");
        
        // Arrange
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetExerciseById_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/exercises/1");
        
        // Arrange
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}