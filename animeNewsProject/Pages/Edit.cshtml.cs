using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
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

            if (UpdatedArticle == null)
            {
                return NotFound();
            }

            return Page();
        }

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