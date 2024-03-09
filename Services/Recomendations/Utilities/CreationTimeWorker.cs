namespace SmartRecipes.Services.Recomendations.Utilities;

public static class CreationTimeWorker
{
    public static string GetStringPeriodRepresentation(DateTime creationTime)
    {
        var now = DateTime.Now;
        if (creationTime.AddDays(7)  > now)
        {
            return "day";
        }
        else if (creationTime.AddDays(30) > now)
        {
            return "week";
        }
        else if (creationTime.AddDays(40) > now)
        {
            return "month";
        }
        else
        {
            return "very-old";
        }
    }
}