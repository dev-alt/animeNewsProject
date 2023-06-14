using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace animeNewsProject.Pages
{

    public class TestingDbModel : PageModel
    {
        private readonly MongoDbService _mongoDbService;
        public bool IsMongoDbConnected { get; private set; }
        public string MongoDbConnectionString { get; private set; }
        public List<string> DatabaseNames { get; set; }

        private readonly BlobStorageService _blobStorageService;
        public string BlobStorageResult { get; set; }
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
        public TestingDbModel(BlobStorageService blobStorageService, MongoDbService mongoDbService)
        {
            _blobStorageService = blobStorageService;
            _mongoDbService = mongoDbService;
            IsMongoDbConnected = IsConnected();
            MongoDbConnectionString = GetMongoDbConnectionString();
            DatabaseNames = GetDatabaseNames();
            BlobStorageConnectionString = blobStorageService.GetConnectionString();
        }


        //public string GetConnectionString()
        //{
        //    var connectionString = _blobStorageService.GetBlobServiceClient();
        //    var formattedConnectionString = connectionString.Replace(";", "; ");
        //    return formattedConnectionString;
        //}

        //public string GetConnectionString()
        //{
        //    var connectionString = _blobStorageService.GetBlobClient().AccountName;
        //    var formattedConnectionString = connectionString.Replace(";", "; ");
        //    return formattedConnectionString;
        //}

        public string GetMongoDbConnectionString()
        {
            var connectionString = _mongoDbService.GetMongoClient().Settings.ToString();
            var formattedConnectionString = connectionString.Replace(";", "; ");
            return formattedConnectionString;
        }



        public bool IsConnected()
        {
            try
            {
                _mongoDbService.GetMongoClient().ListDatabaseNames();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private List<string> GetDatabaseNames()
        {
            try
            {
                return _mongoDbService.GetMongoClient().ListDatabaseNames().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving database names: {ex.Message}");
                Console.WriteLine($"Exception stack trace: {ex.StackTrace}");
                return new List<string>();
            }
        }
    }
}