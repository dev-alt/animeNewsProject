using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IArticleRepository _articleRepository;


        [BindProperty]
        public AnimeArticle UpdatedArticle { get; set; }


        public EditModel(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            UpdatedArticle = new AnimeArticle(); // Initialize the UpdatedArticle property with a non-null value
        }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            UpdatedArticle = await _articleRepository.GetArticleByIdAsync(id);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UpdatedArticle.DocumentId != null)
            {
                var existingArticle = await _articleRepository.GetArticleByIdAsync(UpdatedArticle.DocumentId);

                if (existingArticle == null)
                {
                    return NotFound();
                }

                // Assign the existing comments to the updated article
                UpdatedArticle.Comments = existingArticle.Comments;
            }

            await _articleRepository.UpdateArticleAsync(UpdatedArticle);

            return RedirectToPage("/Index");
        }
    }
}