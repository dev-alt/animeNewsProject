using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }
        public List<Recipe> SearchResults { get; set; }

        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int RecipesPerPage { get; set; } = 6; // Adjust the number of recipes per page
        public int TotalPages { get; set; }


        private readonly ILogger<IndexModel> _logger;
        private readonly MongoDbService _mongoDbService;
        public List<Recipe> CollectionData { get; private set; }
        public IndexModel(ILogger<IndexModel> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        public void OnGet()
        {
            CollectionData = _mongoDbService.GetAllDocuments<Recipe>("recipes");

            if (!string.IsNullOrEmpty(Search))
            {
                SearchResults = CollectionData
                    .Where(r => r.Name.Contains(Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                SearchResults = CollectionData;
            }

            // Apply pagination
            TotalPages = (int)Math.Ceiling(SearchResults.Count / (double)RecipesPerPage);
            SearchResults = SearchResults
                .Skip((CurrentPage - 1) * RecipesPerPage)
                .Take(RecipesPerPage)
                .ToList();
        }

    }
}