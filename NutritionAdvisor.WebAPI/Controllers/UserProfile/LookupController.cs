using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutritionAdvisor.Application.Common.Interfaces;

namespace NutritionAdvisor.WebAPI.Controllers.UserProfile;

[ApiController]
[Route("api/[controller]")]
public class LookupController(IApplicationDbContext context) : ControllerBase
{
    [HttpGet("allergies")]
    public async Task<IActionResult> GetAllergies()
    {
        var allergies = await context.Allergies
            .Select(a => new { a.Id, a.Name })
            .ToListAsync();
            
        return Ok(allergies);
    }

    [HttpGet("diet-preferences")]
    public async Task<IActionResult> GetDietPreferences()
    {
        var diets = await context.DietPreferences
            .Select(d => new { d.Id, Name = d.Name.ToString() }) 
            .ToListAsync();
            
        return Ok(diets);
    }
}