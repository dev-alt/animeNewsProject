using Azure.Storage.Blobs;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace animeNewsProject
{
    /// <summary>
    /// Service class for interacting with Azure Blob Storage.
    /// </summary>
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageService"/> class.
        /// </summary>
        /// <param name="blobServiceClient">The Blob Service client.</param>
        /// <param name="containerName">The name of the Blob Storage container.</param>
        public BlobStorageService(BlobServiceClient blobServiceClient, string containerName)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = containerName;
        }

        /// <summary>
        /// Gets the connection string for the Blob Storage service.
        /// </summary>
        /// <returns>The formatted connection string.</returns>
        public string GetConnectionString()
        {
            var connectionString = _blobServiceClient.Uri.ToString();
            var formattedConnectionString = connectionString.Replace("http://", "https://");
            return formattedConnectionString;
        }

        /// <summary>
        /// Gets the Blob Service client.
        /// </summary>
        /// <returns>The Blob Service client.</returns>
        public BlobServiceClient GetBlobClient()
        {
            return _blobServiceClient;
        }

        /// <summary>
        /// Gets the Blob Service client.
        /// </summary>
        /// <returns>The Blob Service client.</returns>
        public BlobServiceClient GetBlobServiceClient()
        {
            return _blobServiceClient;
        }

        /// <summary>
        /// Uploads a file to Blob Storage asynchronously.
        /// </summary>
        /// <param name="filePath">The local file path to upload.</param>
        /// <param name="blobName">The name of the blob.</param>
        public async Task UploadFileAsync(string filePath, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(filePath);
        }

        /// <summary>
        /// Downloads a file from Blob Storage asynchronously.
        /// </summary>
        /// <param name="blobName">The name of the blob to download.</param>
        /// <param name="downloadPath">The local file path to save the downloaded file.</param>
        public async Task DownloadFileAsync(string blobName, string downloadPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DownloadToAsync(downloadPath);
        }

        // Add more methods for other operations like listing blobs, deleting blobs, etc.
    }
}
