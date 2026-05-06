using System.Net.Http;
using System.Text.Json;
using RecipeFinder.Models;

namespace RecipeFinder.Services
{
    public class SpoonacularRecipeService : RecipeService
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;

        public SpoonacularRecipeService(string apiKey)
        {
            _apiKey = apiKey;
            _client = new HttpClient();
        }

        public override async Task<Recipe> SearchByCuisineAsync(UserPreferences prefs)
        {
            //Implementation to come
            return new Recipe();
        }

        public override async Task<Recipe> SearchByIngredientsAsync(List<string> ingredients, UserPreferences prefs);
        {
            //Implementation to come
            return new Recipe(); 
        }
    }
}