namespace SmartRecipes.Services.Recomendations.Utilities;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ActionTypeValueAttribute : Attribute
{
    public int ActionValue { get; init; }
    public ActionTypeValueAttribute(int actionValue)
    {
        ActionValue = actionValue;
    }
}