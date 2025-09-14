using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests;

[Collection(nameof(ApiTestCollection))]
public class HomeTests(ApiTestFixture fixture)
{
    [Fact]
    public async Task Get_Home_ShouldReturnHelloWorld()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/");
        
        // Arrange
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello, World!", responseString);
    }
}