using FluentResults;
using Kaizen.API.Models;

namespace Kaizen.API.Services;

public interface IExerciseService
{
    public Task<Result<Exercise>> CreateExerciseAsync(string name, IEnumerable<string> muscleGroupCodes);

    public Task<Result<Exercise>> UpdateExerciseAsync(int id, string name, IEnumerable<string> muscleGroupCodes);

    public Task<Exercise?> GetExerciseAsync(int id);

    public Task<IList<Exercise>> GetExercisesAsync();
}