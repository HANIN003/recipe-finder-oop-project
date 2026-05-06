using RecipeFinder.Models;
using RecipeFinder.Services;
using RecipeFinder.Repositories;
using System.Text.RegularExpressions;

namespace RecipeFinder
{
    public class Program
    {
        private static RecipeService _recipeService = null!;
        private static IFavoritesRepository _favoritesRepo = null!;

        public static void Main(string[] args)
        {
            _recipeService = new SpoonacularRecipeService("6a7ecfd7849342fd967db287d2b14b6c");
            _favoritesRepo = new FileFavoritesRepository();

            ShowMenu().Wait();
        }

        private static async Task ShowMenu()
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n---- Recipe Finder ----");
                Console.WriteLine("1. Search by Cuisine");
                Console.WriteLine("2. Search by Ingredients");
                Console.WriteLine("3. View Favorites");
                Console.WriteLine("4. Clear Favorites");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await SearchByCuisine();
                        break;
                    case "2":
                        await SearchByIngredients();
                        break;
                    case "3":
                        ViewFavorites();
                        break;
                    case "4":
                        ClearFavorites();
                        break;
                    case "5":
                        running = false;
                        break;
                    default: Console.WriteLine("Invalid choice. Try again");
                        break;
                }
            }
        }

        private static async Task SearchByCuisine()
        {
            Console.WriteLine("Enter a cuisine (American, Italian, Mexican): ");
            var cuisine = Console.ReadLine();

            Console.WriteLine("Enter intolerances (comma-separted list or leave blank): ");
            var intolerancesInput = Console.ReadLine();

            var prefs = new UserPreferences
            {
                Cuisine = cuisine,
                Intolerances = intolerancesInput?
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.Trim())
                    .ToList() ?? new List<string>()
            };

            try
            {
                var recipe = await _recipeService.SearchByCuisineAsync(prefs);

                Console.WriteLine("\n-----------------------------------------");
                Console.WriteLine($"Recipe Found: {recipe.Title}");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"Time to Make: {recipe.TimeToMake} minutes");
                Console.WriteLine($"Source: {recipe.SourceUrl}");
                var cleanSummary = Regex.Replace(recipe.Summary ?? "", "<.*?>", "");
                Console.WriteLine("\nSummary:\n");
                Console.WriteLine(cleanSummary);
                Console.WriteLine("\n-----------------------------------------");

                Console.WriteLine("\n Save to favorites? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    _favoritesRepo.Save(recipe);
                    Console.WriteLine("Saved!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }        
        }

        private static async Task SearchByIngredients()
        {
            Console.WriteLine("Enter ingredients (comma-separated list): ");
            var input = Console.ReadLine();

            var ingredients = input?
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToList() ?? new List<string>();

            var prefs = new UserPreferences
            {
                Ingredients = ingredients,
            };

            try
            {
                var recipe = await _recipeService.SearchByIngredientsAsync(ingredients, prefs);

                Console.WriteLine("\n-----------------------------------------");
                Console.WriteLine($"\nRecipe Found: {recipe.Title}");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"Time to Make: {recipe.TimeToMake} minutes");
                Console.WriteLine($"Source: {recipe.SourceUrl}");
                var cleanSummary = Regex.Replace(recipe.Summary ?? "", "<.*?>", "");
                Console.WriteLine("\nSummary:\n");
                Console.WriteLine(cleanSummary);
                Console.WriteLine("\n-----------------------------------------");

                Console.WriteLine("\n Save to favorites (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    _favoritesRepo.Save(recipe);
                    Console.WriteLine("Saved!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ViewFavorites()
        {
            Console.WriteLine("\n---- Favorite Recipes ----");

            var favorites = _favoritesRepo.Load();

            if (favorites.Count == 0)
            {
                Console.WriteLine("No favorites saved yet.");
                return;
            }

            int index = 1;
            foreach (var recipe in favorites)
            {
                Console.WriteLine($"\n {index}. {recipe.Title}");
                Console.WriteLine($" Time to Make: {recipe.TimeToMake} minutes");
                Console.WriteLine($" Source: {recipe.SourceUrl}");
                index++;
            }
        }

        private static void ClearFavorites()
        {
            Console.WriteLine("Are you sure you want to clear all favorites (y/n): ");
            var confirm = Console.ReadLine()?.ToLower();

            if (confirm == "y")
            {
                _favoritesRepo.Clear();
                Console.WriteLine("Favorites cleared.");
            }
            else
            {
                Console.WriteLine("Canceled.");
            }
        }
    }
}
