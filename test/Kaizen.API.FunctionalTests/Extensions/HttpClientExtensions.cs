using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity.Data;

namespace Kaizen.API.FunctionalTests.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient Login(this HttpClient httpClient, string email, string password)
    {
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var response = httpClient.PostAsJsonAsync("/auth/login?useCookies=true", loginRequest).Result;
        response.EnsureSuccessStatusCode();
        
        return httpClient;
    }
}