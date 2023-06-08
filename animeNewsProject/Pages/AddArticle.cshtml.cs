using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace animeNewsProject.Pages
{
    public class AddArticleModel : PageModel
    {
        // MongoDB service for database operations
        private readonly MongoDbService _mongoDbService;

        public AddArticleModel(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;

            // Initialize the Article property
            Article = new Entry();
        }

        [BindProperty]
        public Entry Article { get; set; }

        // Handler for the HTTP POST request
        public IActionResult OnPost()
        {
            // Check if the form data is valid
            if (!ModelState.IsValid)
            {
                // Redirect to the Index page if the data is invalid
                return RedirectToPage("Index");
            }

            try
            {
                // Set the DatePublished property to the current UTC time
                Article.DatePublished = DateTime.UtcNow;

                // Specify the collection name
                var collectionName = "articles";

                // Add the Article entry to the database using the MongoDB service
                _mongoDbService.AddEntry(collectionName, Article);

                // Redirect to the Articles page after successful addition
                return RedirectToPage("/Index");
            }
            catch
            {
                // Handle any exception that occurs during the database operation
                ModelState.AddModelError(string.Empty, "Error occurred while adding the article.");

                // Return the page with the error message
                return Page();
            }
        }
    }
}