using animeNewsProject.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    /// <summary>
    /// Represents the page model for the "News" page.
    /// </summary>
    public class NewsModel : PageModel
    {
        /// <summary>
        /// Gets or sets the AnimeArticle object.
        /// </summary>
        public AnimeArticle? Article { get; set; }

        /// <summary>
        /// The repository for managing articles.
        /// </summary>
        private readonly IArticleRepository _yourArticleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsModel"/> class.
        /// </summary>
        /// <param name="yourArticleRepository">The article repository.</param>
        public NewsModel(IArticleRepository yourArticleRepository)
        {
            _yourArticleRepository = yourArticleRepository;
        }

        /// <summary>
        /// Handles the GET request for the page asynchronously.
        /// </summary>
        /// <param name="id">The ID of the article to retrieve.</param>
        /// <returns>The <see cref="IActionResult"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync(string id)
        {
            Article = await _yourArticleRepository.GetArticleByIdAsync(id);

            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }

        /// <summary>
        /// Handles the GET request to edit the article.
        /// </summary>
        /// <param name="id">The ID of the article to edit.</param>
        /// <returns>The <see cref="IActionResult"/> representing the result of the operation.</returns>
        public IActionResult OnGetEdit(string id)
        {
            return RedirectToPage("/Edit", new { id });
        }

        /// <summary>
        /// Handles the POST request to delete the article asynchronously.
        /// </summary>
        /// <param name="id">The ID of the article to delete.</param>
        /// <returns>The <see cref="IActionResult"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var article = await _yourArticleRepository.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            await _yourArticleRepository.DeleteArticleByIdAsync(id);

            return RedirectToPage("/Index");
        }
    }
}