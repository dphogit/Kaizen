using Kaizen.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.AddKaizenCors();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

var logger = app.Logger;
logger.LogInformation("Allowed CORS Origins: {Origins}", string.Join(", ", allowedOrigins));

app.Run();

public partial class Program
{
}