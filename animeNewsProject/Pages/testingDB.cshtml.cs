using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace animeNewsProject.Pages
{
    public class TestingDbModel : PageModel
    {
        private readonly MongoDbService _mongoDbService;

        public bool IsMongoDbConnected { get; private set; }
        public string MongoDbConnectionString { get; private set; }
        public List<string> DatabaseNames { get; set; }

        public TestingDbModel(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
            IsMongoDbConnected = IsConnected();
            MongoDbConnectionString = GetMongoDbConnectionString();
            DatabaseNames = GetDatabaseNames();
        }

        public bool IsConnected()
        {
            try
            {
                _mongoDbService.GetMongoClient().ListDatabaseNames();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetMongoDbConnectionString()
        {
            var connectionString = _mongoDbService.GetMongoClient().Settings.ToString();
            var formattedConnectionString = connectionString.Replace(";", "; ");
            return formattedConnectionString;
        }




        private List<string> GetDatabaseNames()
        {
            try
            {
                return _mongoDbService.GetMongoClient().ListDatabaseNames().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving database names: {ex.Message}");
                Console.WriteLine($"Exception stack trace: {ex.StackTrace}");
                return new List<string>();
            }
        }
    }
}