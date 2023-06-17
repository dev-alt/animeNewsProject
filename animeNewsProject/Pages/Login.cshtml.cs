using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{

    [Authorize]
    public class LoginModel : PageModel
    {
        //[BindProperty]
        //public string Username { get; set; }

        //[BindProperty]
        //public string Password { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            var result = await HttpContext.AuthenticateAsync();

            if (!result.Succeeded)
            {
                // Authentication failed, redirect to the error page
                return RedirectToPage("/Error");
            }

            // Authentication succeeded, redirect to the dashboard
            return RedirectToPage("/Dashboard");
        }
        public void OnGet()
        {
        }
    }
}
