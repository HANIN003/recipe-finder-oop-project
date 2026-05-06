namespace RecipeFinder.Models
{
    public class Recipe
    {
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? SourceUrl { get; set; }
        public int TimeToMake { get; set; }
        public List<string> Cuisines { get; set; } = new();
        public List<string> Diets { get; set; } = new();
        public string? Summary { get; set; }
    }
}