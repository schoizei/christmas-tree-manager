using ChristmasTreeManager.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ChristmasTreeManager.Services;

public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly SecurityService _securityService;
    private ApplicationAuthenticationState? _authenticationState;

    public ApplicationAuthenticationStateProvider(SecurityService securityService)
    {
        _securityService = securityService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        try
        {
            var state = await GetApplicationAuthenticationStateAsync();
            if (state.IsAuthenticated)
            {
                identity = new ClaimsIdentity(state.Claims.Select(c => new Claim(c.Type, c.Value)), "ChristmasTreeManager");
            }
        }
        catch (HttpRequestException ex)
        {
        }

        var result = new AuthenticationState(new ClaimsPrincipal(identity));
        await _securityService.InitializeAsync(result);
        return result;
    }

    private async Task<ApplicationAuthenticationState> GetApplicationAuthenticationStateAsync()
    {
        if (_authenticationState is null)
        {
            _authenticationState = await _securityService.GetAuthenticationStateAsync();
        }

        return _authenticationState;
    }
}