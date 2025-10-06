using System.Net;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Errors;

[Collection(nameof(ApiTestCollection))]
public class ErrorHandlingTests : BaseApiTests
{
    public ErrorHandlingTests(ApiTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task UnexpectedError_ReturnsInternalServerErrorProblemDetails()
    {
        // Arrange
        var client = Fixture.Factory.CreateAuthenticatedClient();
        
        // Act
        var response = await client.GetAsync("/dev/throw");
        
        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}