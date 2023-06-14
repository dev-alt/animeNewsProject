using animeNewsProject.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    public class NewsModel : PageModel
    {
        public AnimeArticle? Article { get; set; }

        private readonly IArticleRepository _yourArticleRepository;

        public NewsModel(IArticleRepository yourArticleRepository)
        {
            _yourArticleRepository = yourArticleRepository;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Article = await _yourArticleRepository.GetArticleByIdAsync(id);

            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnGetEdit(string id)
        {
            return RedirectToPage("/Edit", new { id });
        }

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