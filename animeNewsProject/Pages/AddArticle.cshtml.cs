using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace animeNewsProject.Pages
{
    [Authorize]
    public class AddArticleModel : PageModel
    {

        private readonly MongoDbService _mongoDbService;
        private readonly BlobStorageService _blobStorageService;


        [BindProperty]
        public Entry Article { get; set; }


        public AddArticleModel(MongoDbService mongoDbService, BlobStorageService blobStorageService)
        {
            _mongoDbService = mongoDbService;
            _blobStorageService = blobStorageService;

            // Initialize the Article property
            Article = new Entry();
        }



        public async Task<IActionResult> OnPost(IFormFile imageFile)
        {
            try
            {
                // Check if an image file is provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Set the maximum allowed file size (in bytes)
                    var maxFileSize = 2 * 1024 * 1024; // 2 MB

                    // Generate a unique filename for the image
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                    // Combine the unique filename with the desired storage path
                    var imagePath = Path.Combine("path/to/store/images", uniqueFileName);

                    var imageDirectory = "path/to/store/images";

                    // Check if the file size exceeds the maximum allowed size
                    if (imageFile.Length > maxFileSize)
                    {
                        ModelState.AddModelError(string.Empty, "The uploaded image file exceeds the maximum allowed size.");

                        // Return the page with the error message
                        return Page();
                    }

                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    // Save the image file to a temporary location on the server
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Upload the image file to Azure Blob Storage
                    await _blobStorageService.UploadFileAsync(imagePath, uniqueFileName);

                    // Delete the temporary image file from the server
                    System.IO.File.Delete(imagePath);

                    // Set the Image property of the Article object to the blob URL
                    Article.Image = "https://andbstorage.blob.core.windows.net/andbstorage/" + uniqueFileName;
                }

                Article.Rating = 0;

                // Set the DatePublished property to the current time
                Article.DatePublished = DateTime.Now;

                // Add the Article entry to the database using the MongoDB service
                var collectionName = "articles";
                _mongoDbService.AddEntry(collectionName, Article);

                // Redirect to the Articles page after successful addition
                return RedirectToPage("/Index");
            }
            catch
            {
                // Handle any exception that occurs during the database operation
                ModelState.AddModelError(string.Empty, "Error occurred while adding the article.");

                // Return the page with the error message
                return Page();
            }
        }
    }
}
