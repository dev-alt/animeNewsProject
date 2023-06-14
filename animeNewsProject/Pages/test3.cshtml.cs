using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace animeNewsProject.Pages
{
    public class Test3Model : PageModel
    {


        private readonly BlobStorageService _blobStorageService;
        public string? BlobStorageResult { get; set; }
        public string BlobStorageConnectionString { get; private set; }

        public async Task<IActionResult> TestBlobStorage()
        {
            var blobName = "test-blob.txt";
            var filePath = "path/to/your/file.txt";
            var downloadPath = "path/to/save/downloaded/file.txt";

            try
            {
                await _blobStorageService.UploadFileAsync(filePath, blobName);
                await _blobStorageService.DownloadFileAsync(blobName, downloadPath);

                return Content("File uploaded and downloaded successfully!");
            }
            catch (Exception ex)
            {
                return Content($"Error occurred: {ex.Message}");
            }
        }
        public async Task<IActionResult> OnPostTestBlobStorage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                BlobStorageResult = "No file selected.";
                return Page();
            }

            try
            {
                var filePath = Path.GetTempFileName(); // Save the file to a temporary location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var blobName = file.FileName;

                await _blobStorageService.UploadFileAsync(filePath, blobName);
                await _blobStorageService.DownloadFileAsync(blobName, "DownloadedFile.txt");

                BlobStorageResult = "File uploaded and downloaded successfully.";
            }
            catch (Exception ex)
            {
                BlobStorageResult = $"Error occurred: {ex.Message}";
            }

            return Page();
        }
        public Test3Model(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;

            BlobStorageConnectionString = blobStorageService.GetConnectionString();

        }
    }
}