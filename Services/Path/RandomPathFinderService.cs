

namespace SmartRecipes.Services.PathCalculator;
public class RandomPathFinderService : IPathFinder
{
    public RandomPathFinderService()
    {
        
    }
    public Dictionary<StartDestinationPair, int> AccumulatedValues { get; init; } = new();

    public int CalculateTravelTime(string strartingPoint, string destinationPoint)
    {
        return new Random().Next(5, 30);
    }

    public int CalculateAndAccumulateTravelTime(string startingPoint, string destinationPoint)
    {
        var timeToTravel = CalculateTravelTime(startingPoint, destinationPoint);
        if (AccumulatedValues.Keys.FirstOrDefault(x => x.Destination == destinationPoint) is null) {
            AccumulatedValues[new(startingPoint, destinationPoint)] = timeToTravel;
        }
        return timeToTravel;
    }

    public void AccumulateTravelTime(string startingPoint, string destinationPoint)
    {
        var timeToTravel = CalculateTravelTime(startingPoint, destinationPoint);
        if (AccumulatedValues.Keys.FirstOrDefault(x => x.Destination == destinationPoint) is null)
        {
            AccumulatedValues[new(startingPoint, destinationPoint)] = timeToTravel;
        }
    }
}

