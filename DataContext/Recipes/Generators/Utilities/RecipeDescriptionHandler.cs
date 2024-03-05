namespace SmartRecipes.DataContext.Recipes.Generators.Utilities;

public static class RecipeDescriptionHandler
{
    public static string ToHTML(string description)
    {
        List<string> steps = description.Split('.').ToList();
        for (int i = 0; i < steps.Count(); i++)
        {
            steps[i] = $"""<div id="step-{i + 1}" class="description-step">{steps[i]}.</div>""";
        }
        return String.Format("""<div class="description">{0}</div>""", String.Join("", steps));
    }
}
