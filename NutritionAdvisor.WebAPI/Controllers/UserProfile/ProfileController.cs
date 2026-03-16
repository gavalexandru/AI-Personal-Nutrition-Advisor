using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutritionAdvisor.Application.Allergies.Commands.UpdateUserAllergies;
using NutritionAdvisor.Application.Allergies.Queries.GetUserAllergies;
using NutritionAdvisor.Application.Profiles.Commands.CreateProfile;
using NutritionAdvisor.Application.Profiles.Commands.UpdateProfile;
using NutritionAdvisor.Application.Profiles.Queries.GetProfile;
using NutritionAdvisor.Application.UserDietPreferences.Commands.UpdateUserDietPreferences;
using NutritionAdvisor.Application.UserDietPreferences.Queries.GetUserDietPreferences;

namespace NutritionAdvisor.WebAPI.Controllers.UserProfile;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class ProfileController(IMediator mediator) : ControllerBase
{
    
    // User Profile Endpoints

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = GetUserId();
            var query = new GetUserProfileQuery(userId);
            var profile = await mediator.Send(query);

            if (profile == null)
            {
                return NotFound(new { Message = "Profile not found." });
            }

            return Ok(profile);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] UserProfileRequest request)
    {
        try
        {
            var userId = GetUserId();
            
            var command = new CreateUserProfileCommand(
                userId, 
                request.HeightCm,
                request.WeightKg,
                request.ActivityLevel,
                request.Goal,
                request.DailyCalorieTarget);

            var profileId = await mediator.Send(command);
            return Ok(new { ProfileId = profileId, Message = "Profile created successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileRequest request)
    {
        try
        {
            var userId = GetUserId();

            var command = new UpdateUserProfileCommand(
                userId,
                request.HeightCm,
                request.WeightKg,
                request.ActivityLevel,
                request.Goal,
                request.DailyCalorieTarget);

            await mediator.Send(command);
            return Ok(new { Message = "Profile updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    
    // User Allergies Endpoints
    
    [HttpGet("allergies")]
    public async Task<IActionResult> GetUserAllergies()
    {
        try
        {
            var userId = GetUserId();
            var query = new GetUserAllergiesQuery(userId);
            var allergies = await mediator.Send(query);

            return Ok(allergies);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("allergies")]
    public async Task<IActionResult> UpdateUserAllergies([FromBody] UpdateUserAllergiesRequest request)
    {
        try
        {
            var userId = GetUserId();
            var command = new UpdateUserAllergiesCommand(userId, request.AllergyIds);
            
            await mediator.Send(command);
            
            return Ok(new { Message = "Allergies updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    // User Diet Preferences Endpoints
    
    [HttpGet("diet-preferences")]
    public async Task<IActionResult> GetUserDietPreferences()
    {
        try
        {
            var userId = GetUserId();
            var query = new GetUserDietPreferencesQuery(userId);
            var preferences = await mediator.Send(query);

            return Ok(preferences);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("diet-preferences")]
    public async Task<IActionResult> UpdateUserDietPreferences([FromBody] UpdateUserDietPreferencesRequest request)
    {
        try
        {
            var userId = GetUserId();
            var command = new UpdateUserDietPreferencesCommand(userId, request.DietPreferenceIds);
            
            await mediator.Send(command);
            
            return Ok(new { Message = "Diet preferences updated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub) 
                          ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid token: User ID not found.");
        }
        
        return userId;
    }
}
