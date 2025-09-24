using Kaizen.API.Contracts.Exercises;
using Kaizen.API.Data;
using Kaizen.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MuscleGroupsController
{
    [HttpGet]
    public async Task<MuscleGroupDto[]> GetMuscleGroups(KaizenDbContext dbContext)
    {
        var dtos = await dbContext.MuscleGroups
            .OrderBy(mg => mg.Code)
            .Select(mg => mg.ToMuscleGroupDto())
            .ToArrayAsync();
        
        return dtos;
    }
}