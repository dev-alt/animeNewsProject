using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    public class ArticlesModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }
        public List<AnimeArticle>? SearchResults { get; set; }

        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int ArticlesPerPage { get; set; } = 32; // Adjust the number of articles per page
        public int TotalPages { get; set; }
        public int TotalArticleCount { get; set; } // Property to hold the total count
        private readonly ILogger<ArticlesModel> _logger;
        private readonly MongoDbService _mongoDbService;


        public ArticlesModel(ILogger<ArticlesModel> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        public void OnGet()
        {
            try
            {
                var collectionData = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");

                if (!string.IsNullOrEmpty(Search))
                {
                    // Filter articles based on search query
                    SearchResults = collectionData
                        .Where(r => r.Title != null && r.Title.Contains(Search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                else
                {
                    // No search query, show all articles
                    SearchResults = collectionData;
                }

                // Set the total count
                TotalArticleCount = SearchResults.Count;

                // Apply pagination
                TotalPages = (int)Math.Ceiling(TotalArticleCount / (double)ArticlesPerPage);

                if (CurrentPage < 1)
                {
                    CurrentPage = 1;
                }
                else if (CurrentPage > TotalPages)
                {
                    CurrentPage = TotalPages;
                }

                int startIndex = (CurrentPage - 1) * ArticlesPerPage;
                int endIndex = Math.Min(startIndex + ArticlesPerPage, TotalArticleCount);

                SearchResults = SearchResults
                    .Skip(startIndex)
                    .Take(endIndex - startIndex)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                // Handle the error (e.g., show an error message to the user)
            }
        }
    }
}
