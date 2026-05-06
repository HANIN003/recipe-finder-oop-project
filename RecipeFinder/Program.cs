using RecipeFinder.Models;
using RecipeFinder.Services;
using RecipeFinder.Repositories;

namespace RecipeFinder
{
    public class Program
    {
        private static RecipeService _recipeService;
        private static IFavoritesRepository _favoritsRepo;

        public static void Main(string[] args)
        {
            _recipeService = new SpoonacularRecipeService("6a7ecfd7849342fd967db287d2b14b6c");
            _favoritsRepo = new FileFavoritesRepository();

            ShowMenu();
        }

        private static void ShowMenu()
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
                        SearchByCuisine();
                        break;
                    case "2":
                        SearchByIngredients();
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
                    default: Console.WriteLine("Invalid choice. Try again>");
                        break;
                }
            }
        }

        private static async void SearchByCuisine()
        {
            Console.WriteLine("Enter a cuisine (American, Italian, Mexican): ");
            var cuisine = Console.ReadLine();

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

                Console.WriteLine($"\n Recipe Found: {recipe.Title}");
                Console.WriteLine($"Time to Make: {recipe.TimeToMake} minutes");
                Console.WriteLine($"Source: {recipe.SourceUrl}");
                Console.WriteLine($"Summary: {recipe.Summary}");

                Console.WriteLine("\n Save to favorites? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    _favoritsRepo.Save(recipe);
                    Console.WriteLine("Saved!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }        
        }

        private static void SearchByIngredients()
        {
            Console.WriteLine("Search by Ingredients selected");
            //Code to come
        }

        private static void ViewFavorites()
        {
            Console.WriteLine("View Favorites selected");
            //Code to come
        }

        private static void ClearFavorites()
        {
            Console.WriteLine("Clear favorites selected");
            //Code to come
        }
    }
}
