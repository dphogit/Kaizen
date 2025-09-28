using FluentResults;
using Kaizen.API.Models;
using Kaizen.API.Services.Requests;

namespace Kaizen.API.Services;

public interface IWorkoutService
{
    public Task<Result<Workout>> CreateWorkoutAsync(CreateWorkoutRequest request);

    public Task<IList<Workout>> GetWorkoutsAsync(GetWorkoutsFilters filters);
    
    public Task<Workout?> GetWorkoutAsync(long id);
}