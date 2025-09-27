using Kaizen.API.Contracts.Workouts;
using Kaizen.API.Middleware;
using Kaizen.API.Models;
using Kaizen.API.Services;
using Kaizen.API.Services.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkoutsController(IWorkoutService workoutService, ILogger<WorkoutsController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<Results<Created<WorkoutDto>, ProblemHttpResult>> RecordWorkout(
        [FromBody] RecordWorkoutDto recordWorkoutDto)
    {
        var user = HttpContext.GetCurrentUser();

        var sets = recordWorkoutDto.Sets.Select(s => new CreateWorkoutRequest.Set
        {
            ExerciseId = s.ExerciseId,
            Quantity = s.Quantity,
            Repetitions = s.Repetitions,
            MeasurementUnitCode = s.MeasurementUnitCode,
        }).ToList();

        var createWorkoutRequest = new CreateWorkoutRequest
        {
            Name = recordWorkoutDto.Name,
            PerformedAt = recordWorkoutDto.PerformedAt,
            Sets = sets,
            UserId = user.Id
        };

        var result = await workoutService.CreateWorkoutAsync(createWorkoutRequest);

        if (result.IsFailed)
        {
            var errorMessages = result.Errors.Select(e => e.Message).ToList();

            return TypedResults.Problem(
                title: "One or more validation errors occurred.",
                statusCode: StatusCodes.Status400BadRequest,
                detail: "See the 'errors' field for details.",
                extensions: new Dictionary<string, object?> { { "errors", errorMessages } });
        }

        var createdWorkout = result.Value;

        var location = Url.Action(nameof(GetWorkout), new { id = createdWorkout.Id });
        return TypedResults.Created(location, createdWorkout.ToWorkoutDto());
    }
    
    [HttpGet("{id:long}")]
    public async Task<Results<Ok<WorkoutDto>, ProblemHttpResult>> GetWorkout(long id)
    {
        var workout = await workoutService.GetWorkoutAsync(id);

        if (workout is null)
        {
            logger.LogWarning("Workout not found: {WorkoutId}", id);
            return CreateNotFoundProblem(id);
        }
        
        var currentUser = HttpContext.GetCurrentUser();

        if (workout.UserId != currentUser.Id)
        {
            logger.LogWarning("Workout id {WorkoutId} does not exist for {Email}", id, workout.UserId);
            return CreateNotFoundProblem(id);
        }

        return TypedResults.Ok(workout.ToWorkoutDto());
    }

    private static ProblemHttpResult CreateNotFoundProblem(long id)
    {
        return TypedResults.Problem(
            title: "Workout not found",
            statusCode: StatusCodes.Status404NotFound,
            detail: $"No workout found with the id: {id}");
    }
}