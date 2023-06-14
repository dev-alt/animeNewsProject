using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace animeNewsProject.Pages
{
    public class FeaturesModel : PageModel
    {
<<<<<<< HEAD

=======
        private readonly BlobStorageService _blobStorageService;

        public FeaturesModel(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Test uploading a file
            var filePath = "path/to/your/file.txt";
            var blobName = "test-file.txt";
            await _blobStorageService.UploadFileAsync(filePath, blobName);

            // Test downloading a file
            var downloadPath = "path/to/save/downloaded-file.txt";
            await _blobStorageService.DownloadFileAsync(blobName, downloadPath);

            return Page();
        }
>>>>>>> master
    }
}
