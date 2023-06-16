using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    /// <summary>
    /// Page model for the login page.
    /// </summary>
    [Authorize]
    public class LoginModel : PageModel
    {
        /// <summary>
        /// Handles the POST request for user login.
        /// </summary>
        /// <param name="username">The username provided by the user.</param>
        /// <param name="password">The password provided by the user.</param>
        /// <returns>The action result based on the login result.</returns>
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

        /// <summary>
        /// Validates the user credentials.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if the user is valid, false otherwise.</returns>
        private static bool IsValidUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
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
