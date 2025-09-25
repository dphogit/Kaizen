using Kaizen.API.Data;
using Kaizen.API.FunctionalTests.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kaizen.API.FunctionalTests.Infrastructure;

public class TestWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    public const string TestAdminEmail = "testadmin@kaizen.com";
    public const string TestAdminPassword = "KaizenAdmin123!";

    public const string TestEmail = "testuser@kaizen.com";
    public const string TestPassword = "KaizenUser123!";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configure the DB used by the application from the given connection string.
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(sd => sd.ServiceType == typeof(DbContextOptions<KaizenDbContext>));
            services.Remove(descriptor);

            services.AddSqlServer<KaizenDbContext>(connectionString);
        });
        
        // Override the seeded admin username and password so it is known for testing.
        builder.ConfigureAppConfiguration((context, _) =>
        {
            context.Configuration["AdminUser:Email"] = TestAdminEmail;
            context.Configuration["AdminUser:Password"] = TestAdminPassword;
        });
    }

    /// <summary>
    /// Creates an <b>unauthenticated</b> HTTP client configured with testing defaults.
    /// Useful for testing scenarios around authentication/authorization.
    /// </summary>
    /// <returns></returns>
    public new HttpClient CreateClient()
    {
        // Avoids HTTPs redirection warning in logs from HTTPs Redirection Middleware
        // https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0&pivots=xunit#client-options
        var options = new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        };
        
        return base.CreateClient(options);
    }

    /// <summary>
    /// Creates an <b>authenticated</b> HTTP client with normal user permissions.
    /// </summary>
    /// <returns></returns>
    public HttpClient CreateAuthenticatedClient()
    {
        var client = CreateClient();

        using (var scope = Services.CreateScope())
        {
            IdentitySeeder.SeedUser(scope.ServiceProvider, TestEmail, TestPassword).Wait();
        }

        client.Login(TestEmail, TestPassword);
        
        return client;
    }

    /// <summary>
    /// Creates an <b>authenticated</b> HTTP client with the user <b>admin</b> role.
    /// </summary>
    /// <returns></returns>
    public HttpClient CreateAdminClient()
    {
        var client = CreateClient();
        
        client.Login(TestAdminEmail, TestAdminPassword);
        
        return client;
    }
}