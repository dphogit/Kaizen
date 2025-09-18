using System.Net;
using Kaizen.API.FunctionalTests.Infrastructure;

namespace Kaizen.API.FunctionalTests.Auth;

[Collection(nameof(ApiTestCollection))]
public class LogoutTests(ApiTestFixture fixture) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await fixture.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Logout_Authenticated_RemovesCookie()
    {
        // Arrange
        var client = fixture.Factory.CreateAdminClient();
        
        // Act
        var response = await client.PostAsync("/auth/logout", null);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        // Check for no set-cookie, otherwise if exists make sure it has been invalidated.
        if (response.Headers.TryGetValues("Set-Cookie", out var setCookieValues))
        {
            Assert.Contains(setCookieValues, cookie => cookie.StartsWith("KaizenAuth=;"));
        }
    }

    [Fact]
    public async Task Logout_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.PostAsync("/auth/logout", null);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}