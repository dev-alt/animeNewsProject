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
                var client = new MongoClient(mongodbConnectionString);

                // Specify the database name
                var databaseName = "anime_news_project";

                // Create and return an instance of the MongoDbService, providing the client and database name
                return new MongoDbService(client, databaseName);
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

            // Start the application
            app.Run();
        }
    }
}
