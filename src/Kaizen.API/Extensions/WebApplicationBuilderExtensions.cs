using Azure.Identity;
using Kaizen.API.Data;
using Kaizen.API.Models;
using Kaizen.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace Kaizen.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddKaizen(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            builder.AddKaizenAzure();
        }
        
        return builder.AddKaizenCors()
            .AddKaizenDatabase()
            .AddKaizenAuth()
            .AddKaizenServices()
            .AddKaizenHealthCheck();
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
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization)
                .AllowCredentials());
        });

        return builder;
    }

    private static WebApplicationBuilder AddKaizenDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetRequiredDbConnectionString();

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
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(AuthConstants.Policies.RequireAdminRole, policyBuilder =>
            {
                policyBuilder.RequireRole(AuthConstants.Roles.Admin);
            });

        return builder;
    }

    private static WebApplicationBuilder AddKaizenServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IExerciseService, ExerciseService>();
        builder.Services.AddScoped<IWorkoutService, WorkoutService>();
        
        return builder;
    }

    private static WebApplicationBuilder AddKaizenHealthCheck(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetRequiredDbConnectionString();
        
        builder.Services.AddHealthChecks()
            .AddSqlServer(connectionString, tags: ["ready"]);
        
        return builder;
    }

    private static WebApplicationBuilder AddKaizenAzure(this WebApplicationBuilder builder)
    {
        var vaultUriValue = builder.Configuration.GetValue<string>("Azure:KeyVaultUri");
        
        if (vaultUriValue is null)
        {
            throw new InvalidOperationException("Azure:KeyVaultUri is not configured.");
        }

        builder.Configuration.AddAzureKeyVault(new Uri(vaultUriValue), new ManagedIdentityCredential());

        builder.Logging.AddAzureWebAppDiagnostics();

        return builder;
    }

    private static string GetRequiredDbConnectionString(this ConfigurationManager config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");

        if (connectionString is null)
            throw new InvalidOperationException("No connection string is configured");
        
        return connectionString;
    }
}