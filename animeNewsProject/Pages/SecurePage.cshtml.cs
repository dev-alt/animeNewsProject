using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    [Authorize]
    public class SecurePageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
