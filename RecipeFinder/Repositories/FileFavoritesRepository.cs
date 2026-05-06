using System.Text.Json;
using RecipeFinder.Models;

namespace RecipeFinder.Repositories
{
    public class FileFavoritesRepository : IFavoritesRepository
    {
        private readonly string _filePath = "favorite.json";

        public void Save(Recipe recipe)
        {
            var list = Load();
            list.Add(recipe);

            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }

        public List<Recipe> Load()
        {
            if (!File.Exists(_filePath))
                return new List<Recipe>();

            var json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<Recipe>>(json);
                ?? new List<Recipe>();
        }

        public void Clear();
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }
}