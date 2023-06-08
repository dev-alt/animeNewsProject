using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace animeNewsProject.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public List<AnimeArticle>? SearchResults { get; set; }

        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int ArticlesPerPage { get; set; } = 6; // Adjust the number of articles per page

        public int TotalPages { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly MongoDbService _mongoDbService;

        public List<AnimeArticle> CollectionData { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, MongoDbService mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;

            CollectionData = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");
        }

        public IActionResult OnPostDelete(string id)
        {
            try
            {
                var collectionName = "articles";
                _mongoDbService.DeleteEntry<AnimeArticle>(collectionName, id);

                // Redirect to the Articles page after successful deletion
                return RedirectToPage("/Index");
            }
            catch
            {
                // Handle the exception (e.g., log or display an error message)
                ModelState.AddModelError(string.Empty, "Error occurred while deleting the article.");
                return Page();
            }
        }

        public void OnGet()
        {
            try
            {
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

                // Apply pagination
                TotalPages = (int)Math.Ceiling(SearchResults.Count / (double)ArticlesPerPage);

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
