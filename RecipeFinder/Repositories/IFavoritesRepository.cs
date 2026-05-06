using RecipeFinder.Models;

namespace RecipeFinder.Repositories
{
    public interface IFavoritesRepository
    {
        void Save(Recipe recipe);
        List<Recipe> Load();
        void Clear();
    }
}