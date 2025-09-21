using Kaizen.API.Extensions;
using Kaizen.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddKaizen();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddControllers();

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

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.UseCurrentUserMiddleware();

// All endpoints require auth, besides login which allows anonymous. 
app.MapControllers().RequireAuthorization();

app.Run();

public partial class Program
{
}