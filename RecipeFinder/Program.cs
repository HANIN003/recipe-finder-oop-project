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
            //Replace when API key is generated from Spoonacular
            _recipeService = new SpoonacularRecipeService("API_KEY_HERE");
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

        private static void SearchByCuisine()
        {
            Console.WriteLine("Search by Cuisine selected");
            //Code to come
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
