using Kaizen.API.Data;
using Kaizen.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace Kaizen.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddKaizen(this WebApplicationBuilder builder)
    {
        builder.AddKaizenCors();
        builder.AddKaizenDatabase();
        builder.AddKaizenAuth();

        return builder;
    }

    private static WebApplicationBuilder AddKaizenCors(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetRequiredSection("AllowedOrigins").Get<string[]>();

        if (allowedOrigins is null || allowedOrigins.Length == 0)
            throw new InvalidOperationException("No allowed origins are configured");

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy => policy
                .WithOrigins(allowedOrigins)
                .WithMethods("GET", "POST")
                .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization));
        });

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

    private static WebApplicationBuilder AddKaizenAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityApiEndpoints<KaizenUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<KaizenDbContext>();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "KaizenAuth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

        return builder;
    }
}