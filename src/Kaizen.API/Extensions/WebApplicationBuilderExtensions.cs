namespace Kaizen.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Adds CORS settings for the Kaizen API based on application configuration.
    /// Applies it as a default CORS policy which applies to all endpoints.
    /// </summary>
    /// <param name="builder">The web application builder to apply the CORS configuration on.</param>
    /// <returns>The list of origins configured.</returns>
    /// <exception cref="InvalidOperationException">
    /// If no configuration can be found or there are no allowed origins.
    /// </exception>
    public static string[] AddKaizenCors(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetRequiredSection("AllowedOrigins").Get<string[]>();

        if (allowedOrigins is null || allowedOrigins.Length == 0)
            throw new InvalidOperationException("No allowed origins are configured");

        builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins(allowedOrigins)));

        return allowedOrigins;
    }
}