namespace RecipeFinder.Models
{
    public class UserPreferences
    {
        public string Cuisine { get; set; }
        public List<string> Intolerances { get; set; }
        public List<string> Ingredients { get; set; }

        public UserPreferences()
        {
            Intolerances = new List<string>();
            Ingredients = new List<string>();
        }
    }
}