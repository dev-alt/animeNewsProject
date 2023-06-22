using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace animeNewsProject.Pages
{
    /// <summary>
    /// Represents a page model for testing the database connection and retrieving database information.
    /// </summary>
    public class TestingDbModel : PageModel
    {
        private readonly MongoDbService _mongoDbService;

        /// <summary>
        /// Gets or sets a value indicating whether the MongoDB connection is successful.
        /// </summary>
        public bool IsMongoDbConnected { get; private set; }

        /// <summary>
        /// Gets the MongoDB connection string.
        /// </summary>
        public string MongoDbConnectionString { get; private set; }

        /// <summary>
        /// Gets or sets the list of database names.
        /// </summary>
        public List<string> DatabaseNames { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestingDbModel"/> class.
        /// </summary>
        /// <param name="mongoDbService">The MongoDB service.</param>
        public TestingDbModel(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
            IsMongoDbConnected = IsConnected();
            MongoDbConnectionString = GetMongoDbConnectionString();
            DatabaseNames = GetDatabaseNames();
        }

        /// <summary>
        /// Gets the MongoDB connection string.
        /// </summary>
        /// <returns>The formatted MongoDB connection string.</returns>
        public string GetMongoDbConnectionString()
        {
            var connectionString = _mongoDbService.GetMongoClient().Settings.ToString();
            var formattedConnectionString = connectionString.Replace(";", "; ");
            return formattedConnectionString;
        }

        /// <summary>
        /// Parses the MongoDB connection string and returns key-value pairs of its components.
        /// </summary>
        /// <param name="connectionString">The MongoDB connection string to parse.</param>
        /// <returns>An enumerable of key-value pairs representing the connection string components.</returns>
        public IEnumerable<KeyValuePair<string, string>> ParseConnectionString(string connectionString)
        {
            var pairs = connectionString.Split(';');
            foreach (var pair in pairs)
            {
                var parts = pair.Split('=');
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    if (key.ToLower() == "credentials")
                        continue; // Skip the line with "Credentials"

                    yield return new KeyValuePair<string, string>(key, value);
                }
            }
        }

        /// <summary>
        /// Checks if the MongoDB connection is successful.
        /// </summary>
        /// <returns><c>true</c> if the MongoDB connection is successful; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Retrieves the list of database names.
        /// </summary>
        /// <returns>The list of database names.</returns>
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
