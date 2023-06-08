using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnPost(string username, string password)
        {
            // Perform user authentication and validation logic
            // You can check the credentials against your MongoDB database or any other authentication mechanism you are using

            if (IsValidUser(username, password))
            {
                // Successful login, redirect to a different page
                return RedirectToPage("/Dashboard");
            }
            else
            {
                // Invalid credentials, display error message
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }
        }

        private bool IsValidUser(string username, string password)
        {
            // Implement your authentication logic here
            // Check the credentials against your MongoDB or any other authentication mechanism
            // Return true if the user is valid, otherwise return false

            return true;
        }
        
        public void OnGet()
            {
            }
        }
    }
