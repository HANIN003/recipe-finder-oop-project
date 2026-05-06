using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
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
            var url =
                $"https://api.spoonacular.com/recipes/complexSearch?apiKey={_apiKey}&cuisine={prefs.Cuisine}&intolerances={string.Join(",", prefs.Intolerances)}&addRecipeInformation=true&number=1";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("API request failed.");

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
                throw new Exception("No recipes found.");

            var recipeJson = results[0];

            return new Recipe
            {
                Title = recipeJson.GetProperty("title").GetString() ?? "",
                ImageUrl = recipeJson.GetProperty("image").GetString() ?? "",
                SourceUrl = recipeJson.GetProperty("sourceUrl").GetString() ?? "",
                TimeToMake = recipeJson.GetProperty("readyInMinutes").GetInt32(),
                Summary = recipeJson.GetProperty("summary").GetString() ?? "",
                Cuisines = recipeJson.GetProperty("cuisines").EnumerateArray().Select(c => c.GetString() ?? "").ToList(),
                Diets = recipeJson.GetProperty("diets").EnumerateArray().Select(d => d.GetString() ?? "").ToList()
            };
        }

        public override async Task<Recipe> SearchByIngredientsAsync(List<string> ingredients, UserPreferences prefs)
        {
            var ingredientQuery = string.Join(",", ingredients);

            var url =
                $"https://api.spoonacular.com/recipes/findByIngredients?apiKey={_apiKey}&ingredients={ingredientQuery}&number=1";

            var response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("API request failed.");

            var json = await response.Content.ReadAsStringAsync();

            var results = JsonSerializer.Deserialize<List<JsonElement>>(json);

            if (results == null || results.Count == 0)
                throw new Exception("No recipes found.");

            var recipeJson = results[0];

            var id = recipeJson.GetProperty("id").GetInt32();

            var detailUrl =
                $"https://api.spoonacular.com/recipes/{id}/information?apiKey={_apiKey}&includeNutrition=false";

            var detailResponse = await _client.GetAsync(detailUrl);

            if (!detailResponse.IsSuccessStatusCode)
                throw new Exception("Failed to retrieve recipe details.");

            var detailJson = await detailResponse.Content.ReadAsStringAsync();
            var detailDoc = JsonDocument.Parse(detailJson).RootElement;

            return new Recipe
            {
                Title = detailDoc.GetProperty("title").GetString() ?? "",
                ImageUrl = detailDoc.GetProperty("image").GetString() ?? "",
                SourceUrl = detailDoc.GetProperty("sourceUrl").GetString() ?? "",
                TimeToMake = detailDoc.GetProperty("readyInMinutes").GetInt32(),
                Summary = detailDoc.GetProperty("summary").GetString() ?? "",
                Cuisines = detailDoc.GetProperty("cuisines").EnumerateArray().Select(c => c.GetString() ?? "").ToList(),
                Diets = detailDoc.GetProperty("diets").EnumerateArray().Select(d => d.GetString() ?? "").ToList()
            };
        }
    }
}
