using Kaizen.API.Contracts;
using Kaizen.API.Data;
using Kaizen.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MeasurementUnitsController
{
    [HttpGet]
    public async Task<MeasurementUnitDto[]> GetMeasurementUnits(KaizenDbContext dbContext)
    {
        var dtos = await dbContext.MeasurementUnits
            .OrderBy(mu => mu.Code)
            .Select(mg => mg.ToMeasurementUnitDto())
            .ToArrayAsync();
        
        return dtos;
    }
}