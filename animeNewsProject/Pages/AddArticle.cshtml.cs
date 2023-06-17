using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace animeNewsProject.Pages
{
    //[Authorize]
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
            Article = new Entry(); // Initialize the Article property
        }



        public async Task<IActionResult> OnPost(IFormFile imageFile)
        {
            try
            {
                
                if (imageFile != null && imageFile.Length > 0)// Check if an image file is provided
                {
                    var maxFileSize = 2 * 1024 * 1024; // 2 MB  Set the maximum allowed file size (in bytes)
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName; // Generate a unique filename for the image
                    var imagePath = Path.Combine("path/to/store/images", uniqueFileName); // Combine the unique filename with the desired storage path
                    var imageDirectory = "path/to/store/images";

                    if (imageFile.Length > maxFileSize)// Check if the file size exceeds the maximum allowed size
                    {
                        ModelState.AddModelError(string.Empty, "The uploaded image file exceeds the maximum allowed size.");
                        return Page(); // Return the page with the error message
                    }

                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    using (var fileStream = new FileStream(imagePath, FileMode.Create)) // Save the image file to a temporary location on the server
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    await _blobStorageService.UploadFileAsync(imagePath, uniqueFileName); // Upload the image file to Azure Blob Storage
                    System.IO.File.Delete(imagePath);     // Delete the temporary image file from the server
                    Article.Image = "https://andbstorage.blob.core.windows.net/andbstorage/" + uniqueFileName;// Set the Image property of the Article object to the blob URL
                }

                Article.Rating = 0;
                Article.DatePublished = DateTime.Now; // Set the DatePublished property to the current time
                var collectionName = "articles"; // Add the Article entry to the database using the MongoDB service
                _mongoDbService.AddEntry(collectionName, Article);

                
                return RedirectToPage("/Index"); // Redirect to the Articles page after successful addition
            }
            catch
            {
               
                ModelState.AddModelError(string.Empty, "Error occurred while adding the article."); // Handle any exception that occurs during the database operation
                return Page(); // Return the page with the error message
            }
        }
    }
}
