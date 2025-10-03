using HealthChecks.UI.Client;
using Kaizen.API.Configuration;
using Kaizen.API.Extensions;
using Kaizen.API.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

builder.AddKaizen();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddControllers(options =>
{
    var toKebabCaseConvention = new RouteTokenTransformerConvention(new ToKebabParameterTransformer());
    options.Conventions.Add(toKebabCaseConvention);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed single admin user (me)
await app.SeedAdminUser();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.UseCurrentUserMiddleware();

// liveness probe
app.MapHealthChecks("/healthz/live", new HealthCheckOptions
{
    Predicate = _ => false
});

// readiness probe
app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// All endpoints require auth, besides login which allows anonymous. 
app.MapControllers().RequireAuthorization();

app.Run();

public partial class Program
{
}