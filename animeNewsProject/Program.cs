using animeNewsProject.Pages;
using Azure.Storage.Blobs;
using Microsoft.Identity.Web;
using MongoDB.Driver;

namespace animeNewsProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            }).CreateLogger<Program>();

            logger.LogInformation("Application started. :)");

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();


            var mongoDbConnectionString = configuration.GetConnectionString("MongoDbConnectionString");
            var blobStorageConnectionString = configuration.GetConnectionString("BlobStorageConnectionString");



            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<MongoDbService>(provider =>
            {
                // Create a new MongoDB client with the connection string
                var client = new MongoClient(mongoDbConnectionString);
                // Specify the database name
                var databaseName = "anime_news_project";
                // Create and return an instance of the MongoDbService, providing the client and database name
                var mongoDbService = new MongoDbService(client, databaseName);
                // Log the MongoDB service creation
                var logger = provider.GetRequiredService<ILogger<MongoDbService>>();
                logger.LogInformation("MongoDbService instance created.");

                return mongoDbService;
            });

            builder.Services.AddSingleton<BlobStorageService>(provider =>
            {
                // Create a new BlobServiceClient using the storage connection string
                var blobServiceClient = new BlobServiceClient(blobStorageConnectionString);

                // Specify the container name for storing blobs
                var containerName = "andbstorage"; // Replace with your container name

                // Create and return an instance of the BlobStorageService, providing the BlobServiceClient and container name
                var blobStorageService = new BlobStorageService(blobServiceClient, containerName);

                // Log the BlobStorageService creation
                var logger = provider.GetRequiredService<ILogger<BlobStorageService>>();
                logger.LogInformation("BlobStorageService instance created.");

                return blobStorageService;
            });


            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

            builder.Services.AddRazorPages();

            // Build the application
            var app = builder.Build();


            if (!app.Environment.IsDevelopment())  // If the application is not running in a development environment, configure error handling and enable HTTPS
            {
                // Log error handling configuration
                logger.LogInformation("Configuring error handling and enabling HTTPS.");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection(); //Redirect HTTP requests to HTTPS
            app.UseStaticFiles();
            app.UseRouting();

            app.UseStatusCodePagesWithReExecute("/AccessDenied");
            app.UseAuthentication();
            app.UseAuthorization();

            // Map Razor Pages
            app.MapRazorPages();
            app.MapControllers();

            // Log the application startup completion
            logger.LogInformation("Application startup completed.");

            // Start the application
            app.Run();
        }
    }
}
