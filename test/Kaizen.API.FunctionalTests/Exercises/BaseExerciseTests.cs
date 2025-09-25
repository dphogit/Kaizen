using System.Net.Http.Json;
using System.Text.Json;
using Kaizen.API.Contracts.Exercises;
using Kaizen.API.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.FunctionalTests.Exercises;

public abstract class BaseExerciseTests : IAsyncLifetime
{
    protected readonly ApiTestFixture Fixture;
    
    protected BaseExerciseTests(ApiTestFixture fixture)
    {
       Fixture = fixture; 
    }
    
    public async Task InitializeAsync()
    {
        await Fixture.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
    protected async Task<ExerciseDto> CreateExercise(UpsertExerciseDto request)
    {
        using var client = Fixture.Factory.CreateAdminClient();

        var response = await client.PostAsJsonAsync("/exercises", request);
        response.EnsureSuccessStatusCode();
        
        var exercise = await response.Content.ReadFromJsonAsync<ExerciseDto>();
        Assert.NotNull(exercise);
        
        return exercise;
    }

    protected static async Task AssertResponseHasProblemDetailsErrors(HttpResponseMessage response)
    {
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);

        Assert.True(problem.Extensions.TryGetValue("errors", out var errors));
        var errorsEl = Assert.IsType<JsonElement>(errors);
        Assert.Equal(JsonValueKind.Array, errorsEl.ValueKind);

        Assert.True(errorsEl.EnumerateArray().Any());
    }
}