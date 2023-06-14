using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web.UI;


namespace animeNewsProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a logger instance
            var logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            }).CreateLogger<Program>();

            // Log the application startup
            logger.LogInformation("Application started.");


            DotNetEnv.Env.TraversePath().Load();

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var storageConnectionString = configuration["STORAGE_CONNECTION_STRING"];
            var mongodbConnectionString = configuration["MONGODB_CONNECTION_STRING"];


            // Create a new web application builder
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(options => configuration.Bind("AzureAd", options))
                .AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd")
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches();

            builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    // Set the error message in ViewData
                    context.HttpContext.Items["AuthFailed"] = "Authentication failed.";

                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });


            builder.Services.AddSingleton<MongoDbService>(provider =>
            {
                // Create a new MongoDB client with the connection string
                var client = new MongoClient("mongodb+srv://9957173:mongodb@cluster0.3xvibw0.mongodb.net/?retryWrites=true&w=majority");

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
                var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=andbstorage;AccountKey=bl94CUeTr0DKkz10DJHhi8tBntLffgFqBbV+v7e6C2Wd2xNZZMy2TSdnpMU44iAn/bZsjf+N5X6c+AStVLCT1w==;EndpointSuffix=core.windows.net");

                // Specify the container name for storing blobs
                var containerName = "andbstorage"; // Replace with your container name

                // Create and return an instance of the BlobStorageService, providing the BlobServiceClient and container name
                var blobStorageService = new BlobStorageService(blobServiceClient, containerName);

                // Log the BlobStorageService creation
                var logger = provider.GetRequiredService<ILogger<BlobStorageService>>();
                logger.LogInformation("BlobStorageService instance created.");

                return blobStorageService;
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

            app.UseStatusCodePagesWithReExecute("/AccessDenied");
            app.UseAuthentication();
            // Enable authorization
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
