using Kaizen.API.Contracts.Exercises;
using Kaizen.API.Data;
using Kaizen.API.Middleware;
using Kaizen.API.Models;
using Kaizen.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ExercisesController(
    IExerciseService exerciseService,
    ILogger<ExercisesController> logger) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = AuthConstants.Policies.RequireAdminRole)]
    public async Task<Results<Created<ExerciseDto>, ValidationProblem>> CreateExercise(
        [FromBody] CreateExerciseDto createExerciseDto)
    {
        var user = HttpContext.GetCurrentUser();

        var result = await exerciseService.CreateExerciseAsync(
            createExerciseDto.Name,
            createExerciseDto.MuscleGroupCodes);

        if (result.IsFailed)
        {
            var errorMessages = result.Errors.Select(e => e.Message).ToArray();

            var errors = new Dictionary<string, string[]>
            {
                ["errors"] = errorMessages,
            };

            logger.LogWarning("Exercise creation failed validation for '{Name}': {ErrorMessages}",
                createExerciseDto.Name, errorMessages);

            return TypedResults.ValidationProblem(errors, detail: "See the 'errors' field for details.");
        }

        var exercise = result.Value;

        logger.LogInformation(
            "{Email} created exercise '{Name}' with id: {ExerciseId}", user.Email, exercise.Name, exercise.Id);

        var location = Url.Action(nameof(GetExercise), new { id = exercise.Id });
        return TypedResults.Created(location, exercise.ToExerciseDto());
    }

    [HttpGet("{id:int}")]
    public async Task<Results<Ok<ExerciseDto>, ProblemHttpResult>> GetExercise(int id)
    {
        var exercise = await exerciseService.GetExerciseAsync(id);

        if (exercise is null)
        {
            logger.LogWarning("Exercise not found: {ExerciseId}", id);

            return TypedResults.Problem(
                title: $"No exercise found with id {id}",
                statusCode: StatusCodes.Status404NotFound);
        }

        return TypedResults.Ok(exercise.ToExerciseDto());
    }
}