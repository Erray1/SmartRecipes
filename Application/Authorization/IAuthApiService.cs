﻿using SmartRecipes.Shared.Models.Accounts;

namespace SmartRecipes.Application.Authorization;
public interface IAuthApiService
{
    Task<RegisterResult> Register(RegisterModel request);
    Task<LoginResult> Login(LoginModel request);
    Task Logout();
}

