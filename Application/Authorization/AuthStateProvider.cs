using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SmartRecipes.Application.Authorization;

public class SmartRecipesAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly ILocalStorageService localStorage;

	public SmartRecipesAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        this.httpClient = httpClient;
        this.localStorage = localStorage;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4) {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

	private List<Claim> ParseClaimFromJwt(string jwt)
    {
        List<Claim> claims = new();
        var payload = jwt.Split(".")[1];
        var jsobBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsobBytes);

        keyValuePairs!.TryGetValue(ClaimTypes.Role, out object? roles);

        if (roles is not null)
        {
            if (roles.ToString()!.Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);
                foreach (var role in parsedRoles!)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
            }
            keyValuePairs.Remove(ClaimTypes.Role);
        }
        claims.AddRange(keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString()!)));

        return claims;
    }

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
        var savedToken = await localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrEmpty(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimFromJwt(savedToken), "jwt")));
	}

    public void MarkUserAsLoggedIn(string email)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

	
}