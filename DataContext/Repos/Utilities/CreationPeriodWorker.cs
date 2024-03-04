namespace SmartRecipes.DataContext.Repos.Utilities;
static class CreationPeriodWorker
{
    public static DateTime GetBorder(string? period)
    {
        switch (period)
        {
            case "week":
                return DateTime.Now.AddDays(-7);
            case "month":
                return DateTime.Now.AddMonths(-1);
            case "half-year":
                return DateTime.Now.AddMonths(-6);
            case "year":
                return DateTime.Now.AddYears(-1);
            case null:
            case "":
            case "all-time":
                return DateTime.MinValue;
        }
        return DateTime.Now;
    }
}

