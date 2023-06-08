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



            builder.Services.AddSingleton<MongoDbService>(provider =>
            {
                // Set up the MongoDB connection string
                var connectionString = "mongodb+srv://9957173:mongodb@cluster0.3xvibw0.mongodb.net/?retryWrites=true&w=majority";
                return new MongoDbService(connectionString);
            });

            // Add support for Razor Pages

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Build the application
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            // Redirect HTTP requests to HTTPS
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();

            // Enable routing
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

                app.Run();

                app.Run();

            app.Run();
        }
    }
}
