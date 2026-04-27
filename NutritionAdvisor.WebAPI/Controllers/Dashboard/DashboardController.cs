using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionAdvisor.Application.MealPlans.Commands.GenerateMealPlan;
using NutritionAdvisor.Application.MealPlans.Queries.GetCurrentMealPlan; 
using NutritionAdvisor.Domain.Enums;

namespace NutritionAdvisor.WebAPI.Controllers.Dashboard;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    
    [HttpGet("current-plan")]
    public async Task<IActionResult> GetCurrentPlan()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        var result = await mediator.Send(new GetCurrentMealPlanQuery(userId));
        return Ok(result);
    }

    [HttpPost("generate-plan")]
    public async Task<IActionResult> GeneratePlan([FromBody] PlanType planType)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

        try
        {
            await mediator.Send(new GenerateMealPlanCommand(userId, planType));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}