using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    public class ArticlesModel : PageModel
    {
        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        /// <summary>
        /// Gets or sets the search results.
        /// </summary>
        public List<AnimeArticle>? SearchResults { get; set; }

        // Pagination properties

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of articles per page.
        /// </summary>
        public int ArticlesPerPage { get; set; } = 32; // Adjust the number of articles per page

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total article count.
        /// </summary>
        public int TotalArticleCount { get; set; } // Property to hold the total count

        private readonly ILogger<ArticlesModel> _logger;
        private readonly MongoDbService _mongoDbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticlesModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mongoDbService">The MongoDB service.</param>
        public ArticlesModel(ILogger<ArticlesModel> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        /// <summary>
        /// Handles the HTTP GET request.
        /// </summary>
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
