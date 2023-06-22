using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace animeNewsProject.Pages
{

    public class IndexModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int ArticlesPerPage { get; set; } = 6;
        public int TotalPages { get; set; }
        public int TotalArticleCount { get; set; }
        private readonly ILogger<IndexModel> _logger;
        private readonly MongoDbService _mongoDbService;
        public List<AnimeArticle>? FeaturedArticles { get; set; }
        public List<AnimeArticle> CollectionData { get; private set; }
        public List<AnimeArticle>? SearchResults { get; set; }

        public IndexModel(ILogger<IndexModel> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            CollectionData = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");            // Get all documents from the "articles" collection
        }

        public void OnGet()
        {
            try     // Get all documents from the "articles" collection
            {
                CollectionData = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");

                if (!string.IsNullOrEmpty(Search))
                {
                    // Filter articles based on search query
                    SearchResults = CollectionData
                        .Where(r => r.Title != null && r.Title.Contains(Search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                else // No search query, show all articles
                {
                    SearchResults = CollectionData;
                }

                TotalArticleCount = SearchResults.Count; // Set the total count
                TotalPages = (int)Math.Ceiling(SearchResults.Count / (double)ArticlesPerPage); // Apply pagination

                // Validate current page number
                if (CurrentPage < 1)
                {
                    CurrentPage = 1;
                }
                else if (CurrentPage > TotalPages)
                {
                    CurrentPage = TotalPages;
                }


                // Retrieve the featured articles from the SearchResults list
                FeaturedArticles = SearchResults
                    .OrderByDescending(a => a.Views)
                    .Take(3)
                    .ToList();


                int startIndex = (CurrentPage - 1) * ArticlesPerPage;
                int endIndex = Math.Min(startIndex + ArticlesPerPage, SearchResults.Count);


                // Get the articles for the current page
                SearchResults = SearchResults
                    .Skip(startIndex)
                    .Take(endIndex - startIndex)
                    .ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
            }
        }
    }
}
