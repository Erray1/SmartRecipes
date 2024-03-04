using SmartRecipes.Shared.Models.Accounts;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace SmartRecipes.Application.Authorization;

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider authProvider;
    private readonly ILocalStorageService localStorage;
    public AuthApiService(HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage)
    {
        this.httpClient = httpClient;
        authProvider = authenticationStateProvider;
        this.localStorage = localStorage;
    }

    public async Task<RegisterResult> Register(RegisterModel request)
    {
        var result = await httpClient.PostAsJsonAsync("api/register", request);
        if (!result.IsSuccessStatusCode)
        {
            return new RegisterResult { IsSuccesful = false, Errors = new List<string> { result.StatusCode.ToString() } };
        }
        return new RegisterResult { IsSuccesful = true };
    }

    public async Task<LoginResult> Login(LoginModel request)
    {
        var requestAsJson = JsonSerializer.Serialize(request);
        var response = await httpClient.PostAsync("api/login",
            new StringContent(requestAsJson, Encoding.UTF8, "application/json"));

        var result = JsonSerializer.Deserialize<LoginResult>(
            await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // ???

        if (!response.IsSuccessStatusCode) return result!;

        await localStorage.SetItemAsync("authToken", result!.Token);

        ((SmartRecipesAuthenticationStateProvider)authProvider).MarkUserAsLoggedIn(request.Email!);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

        return result!;
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("authToken");
        ((SmartRecipesAuthenticationStateProvider)authProvider).MarkUserAsLoggedOut();
        httpClient.DefaultRequestHeaders.Authorization = null;
    }


}

