using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts;
using Kaizen.API.Extensions;
using Kaizen.API.FunctionalTests.Infrastructure;
using Kaizen.API.Models;

namespace Kaizen.API.FunctionalTests.MeasurementUnits;

[Collection(nameof(ApiTestCollection))]
public class GetMeasurementUnitTests(ApiTestFixture fixture)
{
    // No resetting of DB done as data does not change at run time.
    
    [Fact]
    public async Task GetMuscleGroups_Authenticated_ReturnsMuscleGroups()
    {
        // Arrange
        var client = fixture.Factory.CreateAuthenticatedClient();
        
        var expectedDtos = DbContextExtensions.AppMeasurementUnits.ToMeasurementUnitDtos();
        
        // Act
        var response = await client.GetAsync("/measurement-units");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var dtos = await response.Content.ReadFromJsonAsync<MeasurementUnitDto[]>();
        Assert.NotNull(dtos);

        var difference = expectedDtos.Except(dtos);
        Assert.Empty(difference);
    }

    [Fact]
    public async Task GetMeasurementUnits_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/measurement-units");
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}