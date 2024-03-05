using SmartRecipes.Shared.Models.Accounts;

namespace SmartRecipes.Application.Authorization;
public interface IAuthAPIClient
{
    Task<RegisterResult> Register(RegisterModel request);
    Task<LoginResult> Login(LoginModel request);
    Task Logout();
}

