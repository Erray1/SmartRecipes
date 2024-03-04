namespace SmartRecipes.Services.PathCalculator;
public interface IPathFinder
{
    public Dictionary<StartDestinationPair, int> AccumulatedValues { get; init; }
    public int CalculateTravelTime(string startingPoint, string destinationPoint);
    public int CalculateAndAccumulateTravelTime(string startingPoint, string destinationPoint);
    public void AccumulateTravelTime(string startingPoint, string destinationPoint);
}

