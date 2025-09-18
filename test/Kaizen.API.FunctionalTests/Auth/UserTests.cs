using System.Net;
using System.Net.Http.Json;
using Kaizen.API.Contracts;
using Kaizen.API.Data;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Auth;

[Collection(nameof(ApiTestCollection))]
public class UserTests(ApiTestFixture fixture) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await fixture.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
    [Fact]
    public async Task GetMe_NotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/auth");
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMe_Authenticated_ReturnsMyUserData()
    {
        // Arrange
        var client = fixture.Factory.CreateAdminClient();

        // Act
        var response = await client.GetAsync("/auth");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var kaizenUserDto = await response.Content.ReadFromJsonAsync<KaizenUserDto>();
        Assert.NotNull(kaizenUserDto);
        
        Assert.Equal(TestWebApplicationFactory.TestAdminEmail, kaizenUserDto.Email);
        
        Assert.Single(kaizenUserDto.Roles);
        Assert.Equal(RoleConstants.Admin, kaizenUserDto.Roles[0]);
    }
}