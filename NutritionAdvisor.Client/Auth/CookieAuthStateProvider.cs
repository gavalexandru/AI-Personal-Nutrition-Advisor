using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace NutritionAdvisor.Client.Auth;

public class CookieAuthStateProvider(HttpClient httpClient) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            
            var userResponse = await httpClient.GetFromJsonAsync<UserInfoDto>("api/auth/me");

            if (userResponse != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userResponse.Email),
                    new Claim(ClaimTypes.Name, userResponse.FullName)
                };
                var identity = new ClaimsIdentity(claims, "Cookies");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
        catch
        {
            // 401 - Unauthorized, it doesn t exist a valid cookie
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void NotifyUserAuthentication()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }
}