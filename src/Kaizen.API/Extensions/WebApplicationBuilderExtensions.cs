using Kaizen.API.Data;
using Kaizen.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Kaizen.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddKaizenServices(this WebApplicationBuilder builder)
    {
        builder.AddKaizenCors();
        builder.AddKaizenDatabase();

        builder.Services.AddIdentityApiEndpoints<KaizenUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<KaizenDbContext>();

        return builder;
    }

    private static WebApplicationBuilder AddKaizenCors(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetRequiredSection("AllowedOrigins").Get<string[]>();

        if (allowedOrigins is null || allowedOrigins.Length == 0)
            throw new InvalidOperationException("No allowed origins are configured");

        builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins(allowedOrigins)));

        return builder;
    }

    private static WebApplicationBuilder AddKaizenDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (connectionString is null)
            throw new InvalidOperationException("No connection string is configured");

        builder.Services.AddSqlServer<KaizenDbContext>(connectionString);

        return builder;
    }
}