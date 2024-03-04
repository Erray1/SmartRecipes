namespace SmartRecipes.Shared.Models.Accounts;

public sealed class RegisterResult
{
    public bool IsSuccesful { get; set; }
    public List<string>? Errors { get; set; }
}