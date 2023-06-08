using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace animeNewsProject.Pages
{
    public class AddArticleModel : PageModel
    {

        private readonly MongoDbService _mongoDbService;

        public AddArticleModel(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
            Article = new Entry();
        }

        [BindProperty]
        public Entry Article { get; set; }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("Index");
            }

            try
            {
                Article.DatePublished = DateTime.UtcNow;

                var collectionName = "articles";
                _mongoDbService.AddEntry(collectionName, Article);

                // Redirect to the Articles page after successful addition
                return RedirectToPage("/Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error occurred while adding the article.");
                return Page();
            }
        }


    }
}