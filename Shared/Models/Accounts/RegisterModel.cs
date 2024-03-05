using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace SmartRecipes.Shared.Models.Accounts;
public sealed class RegisterModel
{
    [Email]
    public string Email { get; set; }
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина от 6 до 50")]
    public string Password { get; set; }
    [Compare("Password", ErrorMessage = "Указанные пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}

