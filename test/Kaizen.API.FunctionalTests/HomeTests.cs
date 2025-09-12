using Microsoft.AspNetCore.Mvc.Testing;

namespace Kaizen.API.FunctionalTests;

public class HomeTests
{
    [Fact]
    public async Task Get_Home_ShouldReturnHelloWorld()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/");
        
        // Arrange
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello, World!", responseString);
    }
}