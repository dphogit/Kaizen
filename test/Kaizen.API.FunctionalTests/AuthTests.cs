using System.Net;
using System.Net.Http.Json;
using Kaizen.API.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity.Data;

namespace Kaizen.API.FunctionalTests;

[Collection(nameof(ApiTestCollection))]
public class AuthTests(ApiTestFixture fixture)
{
    [Fact]
    public async Task Login_ValidCredentials_ReturnsAccessToken()
    {
        // Arrange
        await fixture.ResetDatabaseAsync();
        
        var loginRequest = new LoginRequest
        {
            Email = TestWebApplicationFactory.TestAdminEmail,
            Password = TestWebApplicationFactory.TestAdminPassword
        };

        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.PostAsJsonAsync("/auth/login", loginRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        
        var accessTokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
        Assert.Equal("Bearer", accessTokenResponse?.TokenType);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        await fixture.ResetDatabaseAsync();

        var loginRequest = new LoginRequest
        {
            Email = TestWebApplicationFactory.TestAdminEmail,
            Password = "IncorrectKaizenAdminPassword"
        };
        
        var client = fixture.Factory.CreateClient();
        
        // Act
        var response = await client.PostAsJsonAsync("/auth/login", loginRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}