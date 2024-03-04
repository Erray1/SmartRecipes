namespace SmartRecipes.Services.Recomendations.Utilities;

public class TokenWeightAndCategories
{
	public int Weight { get; private set; }
	public Dictionary<string, int> CategoriesWeights { private get; init; }

    public TokenWeightAndCategories(int weight, string categoryName)
    {
		Weight = weight;
		CategoriesWeights = new Dictionary<string, int>() { { categoryName, weight > 0 ? 1 : -1 } };
	} 
    

    public void Update(int weight, string category)
	{
		Weight += weight;
		int categoryPreferenceShift = weight > 0 ? 1 : -1;
		if (!CategoriesWeights.ContainsKey(category))
		{
			CategoriesWeights[category] = categoryPreferenceShift;
		}
		CategoriesWeights[category] += categoryPreferenceShift;
	}
	public string GetMostPreferencedCategory()
	{
		return CategoriesWeights.MaxBy(x => x.Value).Key;
	}
}
