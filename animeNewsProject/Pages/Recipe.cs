using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace animeNewsProject.Pages
{
    public class Recipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RecipeId { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Ingredients")]
        public List<string> Ingredients { get; set; }

        [BsonElement("PrepTimeInMinutes")]
        public int PrepTimeInMinutes { get; set; }
        public Recipe(string name, List<string> ingredients, int prepTime)
        {
            this.Name = name;
            this.Ingredients = ingredients;
            this.PrepTimeInMinutes = prepTime;
        }
        public Recipe()
        {
        }
        /// <summary>
        /// This static method is just here so we have a convenient way
        /// to generate sample recipe data.
        /// </summary>
        /// <returns>A list of Recipes</returns>       
        public static List<Recipe> GetRecipes()
        {
            return new List<Recipe>()
            {
                new Recipe("elotes", new List<string>(){"corn", "mayonnaise", "cotija cheese", "sour cream", "lime" }, 35),
                new Recipe("loco moco", new List<string>(){"ground beef", "butter", "onion", "egg", "bread bun", "mushrooms" }, 54),
                new Recipe("patatas bravas", new List<string>(){"potato", "tomato", "olive oil", "onion", "garlic", "paprika" }, 80),
                new Recipe("fried rice", new List<string>(){"rice", "soy sauce", "egg", "onion", "pea", "carrot", "sesame oil" }, 40),
            };
        }
    }
}
