using SmartRecipes.Services.SearchEngines;

namespace SmartRecipes.Services.Recomendations.Utilities;

public static class SearchTokensGetters
{
    public static ISearchTokensGetter CreateNew(SearchProperties searchProperty)
    {
        switch (searchProperty)
        {
            case SearchProperties.Name:
                return new SearchTokensGetterByName();
            default:
                throw new NotImplementedException("Пока можно использовать только поиск по имени");
        }
    }
}
