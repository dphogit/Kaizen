using Kaizen.API.Contracts.Exercises;
using Kaizen.API.Data;
using Kaizen.API.Middleware;
using Kaizen.API.Models;
using Kaizen.API.Services;
using Kaizen.API.Services.Results;
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
    public async Task<Results<Created<ExerciseDto>, ProblemHttpResult>> CreateExercise(
        [FromBody] UpsertExerciseDto upsertExerciseDto)
    {
        var user = HttpContext.GetCurrentUser();

        var result = await exerciseService.CreateExerciseAsync(
            upsertExerciseDto.Name,
            upsertExerciseDto.MuscleGroupCodes);

        if (result.IsFailed)
        {
            var errorMessages = result.Errors.Select(e => e.Message).ToArray();
            var problemResponse = CreateExerciseValidationProblem(errorMessages);

            logger.LogWarning("Exercise creation failed validation for '{Name}': {ErrorMessages}",
                upsertExerciseDto.Name, errorMessages);

            return problemResponse;
        }

        var exercise = result.Value;

        logger.LogInformation(
            "{Email} created exercise '{Name}' with id: {ExerciseId}", user.Email, exercise.Name, exercise.Id);

        var location = Url.Action(nameof(GetExercise), new { id = exercise.Id });
        return TypedResults.Created(location, exercise.ToExerciseDto());
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AuthConstants.Policies.RequireAdminRole)]
    public async Task<Results<Ok<ExerciseDto>, ProblemHttpResult, ValidationProblem>> UpdateExercise(
        [FromRoute] int id,
        [FromBody] UpsertExerciseDto updateExerciseDto)
    {
        var user = HttpContext.GetCurrentUser();
        
        var result = await exerciseService.UpdateExerciseAsync(
            id,
            updateExerciseDto.Name,
            updateExerciseDto.MuscleGroupCodes);

        if (result.IsFailed)
        {
            if (result.HasError<NotFoundError>())
            {
                logger.LogWarning("Exercise update failed. Could not find id: {ExerciseId}", id);
                return CreateExerciseNotFoundProblem(id);
            }
            
            var errorMessages = result.Errors.Select(e => e.Message).ToArray();
            var problemResponse = CreateExerciseValidationProblem(errorMessages);

            logger.LogWarning("Exercise updated failed validation for id: {ExerciseId}: {ErrorMessages}",
                id, errorMessages);

            return problemResponse;
        }
        
        logger.LogInformation("{Email} updated exercise with id: {ExerciseId}", user.Email, id);
        return TypedResults.Ok(result.Value.ToExerciseDto());
    }

    [HttpGet]
    public async Task<Ok<ExerciseDto[]>> GetExercises()
    {
        var exercises = await exerciseService.GetExercisesAsync();
        return TypedResults.Ok(exercises.ToExerciseDtos().ToArray());
    }

    [HttpGet("{id:int}")]
    public async Task<Results<Ok<ExerciseDto>, ProblemHttpResult>> GetExercise(int id)
    {
        var exercise = await exerciseService.GetExerciseAsync(id);

        if (exercise is null)
        {
            logger.LogWarning("Exercise not found: {ExerciseId}", id);
            return CreateExerciseNotFoundProblem(id);
        }

        return TypedResults.Ok(exercise.ToExerciseDto());
    }

    private static ProblemHttpResult CreateExerciseNotFoundProblem(int id)
    {
        return TypedResults.Problem(
            title: "Exercise not found",
            statusCode: StatusCodes.Status404NotFound,
            detail: $"No exercise found with the id: {id}");
    }

    private static ProblemHttpResult CreateExerciseValidationProblem(string[] errorMessages)
    {
        return TypedResults.Problem(
            title: "One or more validation errors occurred.",
            statusCode: StatusCodes.Status400BadRequest,
            detail: "See the 'errors' field for details.",
            extensions: new Dictionary<string, object?> { { "errors", errorMessages } });
    }
}