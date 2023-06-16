using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace animeNewsProject.Pages
{
    /// <summary>
    /// The page model for the "Index" page.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Gets or sets the search query entered by the user.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        /// <summary>
        /// Gets or sets the list of search results.
        /// </summary>
        public List<AnimeArticle>? SearchResults { get; set; }

        /// <summary>
        /// Gets or sets the current page number for pagination.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of articles to display per page.
        /// </summary>
        public int ArticlesPerPage { get; set; } = 6;

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total count of articles.
        /// </summary>
        public int TotalArticleCount { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly MongoDbService _mongoDbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="mongoDbService">The MongoDB service.</param>
        public IndexModel(ILogger<IndexModel> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;

            // Get all documents from the "articles" collection
            CollectionData = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");
        }

        /// <summary>
        /// Gets or sets the collection data.
        /// </summary>
        public List<AnimeArticle> CollectionData { get; private set; }

        /// <summary>
        /// Handles the GET request for the "Index" page.
        /// </summary>
        public void OnGet()
        {
            try
            {
                // Get all documents from the "articles" collection
                CollectionData = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");

                if (!string.IsNullOrEmpty(Search))
                {
                    // Filter articles based on search query
                    SearchResults = CollectionData
                        .Where(r => r.Title != null && r.Title.Contains(Search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                else
                {
                    // No search query, show all articles
                    SearchResults = CollectionData;
                }

                // Set the total count
                TotalArticleCount = SearchResults.Count;

                // Apply pagination
                TotalPages = (int)Math.Ceiling(SearchResults.Count / (double)ArticlesPerPage);

                // Validate current page number
                if (CurrentPage < 1)
                {
                    CurrentPage = 1;
                }
                else if (CurrentPage > TotalPages)
                {
                    CurrentPage = TotalPages;
                }

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
                // Handle the error (e.g., show an error message to the user)
            }
        }
    }
}
