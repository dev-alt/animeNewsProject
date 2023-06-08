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
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddSingleton<MongoDbService>(provider =>
            {
                var connectionString = "mongodb+srv://9957173:mongodb@cluster0.3xvibw0.mongodb.net/?retryWrites=true&w=majority";
                var client = new MongoClient(connectionString);
                var databaseName = "anime_news_project"; // Specify your database name
                return new MongoDbService(client, databaseName);
            });





            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
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