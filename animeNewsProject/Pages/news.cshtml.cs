using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    public class NewsModel : PageModel
    {
        private readonly IArticleRepository _yourArticleRepository;

        [BindProperty]
        public string? Comment { get; set; }

        [BindProperty]
        public string? CommentText { get; set; }
        public AnimeArticle? Article { get; set; }

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
            // Check if the authenticated user belongs to the "Admin" role
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            await _yourArticleRepository.GetArticleByIdAsync(id);

            await _yourArticleRepository.DeleteArticleByIdAsync(id);

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(string id, string comment)
        {
            var article = await _yourArticleRepository.GetArticleByIdAsync(id);

            article.Comments ??= new List<string>();

            article.Comments.Remove(comment);

            await _yourArticleRepository.UpdateArticleAsync(article);

            return RedirectToPage(new { id });
        }


        public async Task<IActionResult> OnPostAsync(string id)
        {
            Article = await _yourArticleRepository.GetArticleByIdAsync(id);

            if (Article == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(CommentText))
            {
                ModelState.AddModelError(string.Empty, "Please enter a comment.");
                return Page();
            }

            // Add the comment to the article
            Article.Comments.Add(CommentText);

            // Save the updated article to the repository
            await _yourArticleRepository.UpdateArticleAsync(Article);

            // Redirect back to the news page
            return RedirectToPage(new { id });
        }


    }

}
