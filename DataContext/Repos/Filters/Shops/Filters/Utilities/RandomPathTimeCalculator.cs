namespace SmartRecipes.DataContext.Repos.Filters.Shops.Filters.Utilities;

public class RandomPathTimeCalculator
{
    private Dictionary<string, int> calculatedPaths { get; set; } = new();
    private int getPathTimeInMunutes(string startAddress, string shopAddress)
    {
        var time = new Random().Next(5, 30);
        calculatedPaths[shopAddress] = time;
        return time;
    }
    public int GetExistingOrCalculateTime(string startAddress, string shopAddress)
    {
        if (calculatedPaths.ContainsKey(shopAddress)) return calculatedPaths[shopAddress];
        return getPathTimeInMunutes(startAddress, shopAddress);
    }
    public int GetExistingTime(string shopAddress)
    {
        return calculatedPaths[shopAddress];
    }
}
