using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    public class EditModel : PageModel
    {
        private readonly IArticleRepository _articleRepository;

        /// <summary>
        /// Gets or sets the updated article.
        /// </summary>
        [BindProperty]
        public AnimeArticle UpdatedArticle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="articleRepository">The article repository.</param>
        public EditModel(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            UpdatedArticle = new AnimeArticle(); // Initialize the UpdatedArticle property with a non-null value
        }

        /// <summary>
        /// Handles the HTTP GET request.
        /// </summary>
        /// <param name="id">The ID of the article to edit.</param>
        /// <returns>The IActionResult representing the page result.</returns>
        public async Task<IActionResult> OnGetAsync(string id)
        {
            UpdatedArticle = await _articleRepository.GetArticleByIdAsync(id);

            if (UpdatedArticle == null)
            {
                return NotFound();
            }

            return Page();
        }

        /// <summary>
        /// Handles the HTTP POST request.
        /// </summary>
        /// <returns>The IActionResult representing the page result.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _articleRepository.UpdateArticleAsync(UpdatedArticle);

            return RedirectToPage("/Index");
        }
    }
}