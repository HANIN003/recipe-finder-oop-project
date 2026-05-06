using RecipeFinder.Models;

namespace RecipeFinder.Services
{
    public abstract class RecipeService
    {
        public abstract Task<Recipe> SearchByCuisineAsync(UserPreferences pref);
        public abstract Task<Recipe> SearchByIngredientsAsync(List<string> ingredients, UserPreferences pref);
    }
}