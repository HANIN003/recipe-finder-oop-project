namespace RecipeFinder.Models
{
    public class UserPreferences
    {
        public string? Cuisine { get; set; }
        public List<string> Intolerances { get; set; } = new();
        public List<string> Ingredients { get; set; } = new();
    }
}