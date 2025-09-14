using Kaizen.API.Data;

namespace Kaizen.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedAdminUser(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();
        
        var newAdminCreated = await IdentitySeeder.SeedAdminUser(services, config);

        if (newAdminCreated)
        {
            app.Logger.LogInformation("Admin user seeded.");
            return;
        }
        
        app.Logger.LogInformation("Admin user already exists. Seeding skipped.");
    }
}