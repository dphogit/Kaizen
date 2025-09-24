using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts.Exercises;
using Kaizen.API.Extensions;
using Kaizen.API.FunctionalTests.Infrastructure;
using Kaizen.API.Models;

namespace Kaizen.API.FunctionalTests.Exercises;

[Collection(nameof(ApiTestCollection))]
public class GetMuscleGroupTests(ApiTestFixture fixture)
{
    // No resetting of DB done as muscles data does not change at run time.
    
    [Fact]
    public async Task GetMuscleGroups_Authenticated_ReturnsMuscleGroups()
    {
        // Arrange
        var client = fixture.Factory.CreateAuthenticatedClient();
        
        var expectedDtos = DbContextExtensions.DefaultMuscleGroups.ToMuscleGroupDtos();
        
        // Act
        var response = await client.GetAsync("/muscle-groups");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var dtos = await response.Content.ReadFromJsonAsync<MuscleGroupDto[]>();
        Assert.NotNull(dtos);

        var difference = expectedDtos.Except(dtos);
        Assert.Empty(difference);
    }

    [Fact]
    public async Task GetMuscleGroups_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/muscle-groups");
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}