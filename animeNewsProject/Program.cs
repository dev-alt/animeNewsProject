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
                var client = new MongoClient(connectionString);
                var databaseName = "anime_news_project"; // Specify your database name
                return new MongoDbService(client, databaseName);
            });





            builder.Services.AddRazorPages();

            // Build the application
            var app = builder.Build();

            // Configure the application

            // If the application is not running in a development environment, configure error handling and enable HTTPS
            if (!app.Environment.IsDevelopment())
            // Redirect HTTP requests to HTTPS
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();

            // Enable routing
            }

            app.UseHttpsRedirection();

            // Serve static files (e.g., CSS, JavaScript, images) from wwwroot folder
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();


            app.MapGet("/{page?}", async (HttpContext context) =>
            {
                var page = context.Request.RouteValues["page"]?.ToString() ?? "1";
                await context.Response.WriteAsync($"Page: {page}");
            });


                app.Run();
        }
    }
}
