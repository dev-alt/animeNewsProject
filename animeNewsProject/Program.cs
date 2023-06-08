using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace animeNewsProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a new web application builder
            var builder = WebApplication.CreateBuilder(args);

            // Configure services and register dependencies

            builder.Services.AddSingleton<MongoDbService>(provider =>
            {
                // Set up the MongoDB connection string
                var connectionString = "mongodb+srv://9957173:mongodb@cluster0.3xvibw0.mongodb.net/?retryWrites=true&w=majority";

                // Create a new MongoDB client with the connection string
                var client = new MongoClient(connectionString);

                // Specify the database name
                var databaseName = "anime_news_project";

                // Create and return an instance of the MongoDbService, providing the client and database name
                return new MongoDbService(client, databaseName);
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

            // Enable authorization
            app.UseAuthorization();

            // Map Razor Pages
            app.MapRazorPages();

            // Define a route for handling HTTP GET requests with a page parameter
            app.MapGet("/{page?}", async (HttpContext context) =>
            {
                // Extract the page parameter from the route values, defaulting to "1" if not provided
                var page = context.Request.RouteValues["page"]?.ToString() ?? "1";

                // Send a response with the page number
                await context.Response.WriteAsync($"Page: {page}");
            });

            // Start the application
            app.Run();
        }
    }
}
