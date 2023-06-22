using Azure.Storage.Blobs;

namespace animeNewsProject
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(BlobServiceClient blobServiceClient, string containerName)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = containerName;
        }

        public string GetConnectionString()
        {
            var connectionString = _blobServiceClient.Uri.ToString();
            var formattedConnectionString = connectionString.Replace("http://", "https://");
            return formattedConnectionString;
        }

        public BlobServiceClient GetBlobClient()
        {
            return _blobServiceClient;
        }

        public BlobServiceClient GetBlobServiceClient()
        {
            return _blobServiceClient;
        }

        public async Task UploadFileAsync(string filePath, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(filePath);
        }

        public async Task DownloadFileAsync(string blobName, string downloadPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DownloadToAsync(downloadPath);
        }
    }
}
