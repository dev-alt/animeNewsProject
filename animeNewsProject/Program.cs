using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;

namespace animeNewsProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load();

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var storageConnectionString = configuration["STORAGE_CONNECTION_STRING"];
            var mongodbConnectionString = configuration["MONGODB_CONNECTION_STRING"];

            
            // Create a new web application builder
            var builder = WebApplication.CreateBuilder(args);

            // Configure services and register dependencies

            builder.Services.AddSingleton<MongoDbService>(provider =>
            {


                // Create a new MongoDB client with the connection string
                var client = new MongoClient(mongodbConnectionString);

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
                var blobServiceClient = new BlobServiceClient(storageConnectionString);

                // Specify the container name for storing blobs
                var containerName = "andbstorage"; // Replace with your container name

                // Create and return an instance of the BlobStorageService, providing the BlobServiceClient and container name
                return new BlobStorageService(blobServiceClient, containerName);
            });






            // Add scoped services
            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();


            // Add support for Razor Pages
            builder.Services.AddRazorPages();

            // Build the application
            var app = builder.Build();

            // Configure the application

            // If the application is not running in a development environment, configure error handling and enable HTTPS
            if (!app.Environment.IsDevelopment())
            {
                // Log error handling configuration
                logger.LogInformation("Configuring error handling and enabling HTTPS.");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Redirect HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Serve static files (e.g., CSS, JavaScript, images) from wwwroot folder
            app.UseStaticFiles();

            // Enable routing
            app.UseRouting();

            app.UseAuthentication();
            // Enable authorization
            app.UseAuthorization();

            // Map Razor Pages
            app.MapRazorPages();
            app.MapControllers();

            // Log the application startup completion
            logger.LogInformation("Application startup completed.");

            // Define a route for handling HTTP GET requests with a page parameter
            //app.MapGet("/{page?}", async (HttpContext context) =>
            //{
            //    // Extract the page parameter from the route values, defaulting to "1" if not provided
            //    var page = context.Request.RouteValues["page"]?.ToString() ?? "1";

            //    // Send a response with the page number
            //    await context.Response.WriteAsync($"Page: {page}");
            //});
     


            // Start the application
            app.Run();
        }
    }
}
