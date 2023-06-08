using MongoDB.Driver;

namespace animeNewsProject
{

    public class MongoDbService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDbService(string connectionString)
        {
<<<<<<< Updated upstream
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("myDatabase");
=======
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _database = _client.GetDatabase(databaseName);
        }

        public IMongoClient GetMongoClient()
        {
            return _client;
>>>>>>> Stashed changes
        }

        // Add methods for your specific database operations

        // Example method: Get all documents from a collection
        public List<T> GetAllDocuments<T>(string collectionName)
<<<<<<< Updated upstream
        {
            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(_ => true).ToList();
        }
    }
}

=======
        {
            var collection = _database.GetCollection<T>(collectionName);

            try
            {
                // Retrieve all documents from the collection
                return collection.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log, throw, or return an empty list)
                Console.WriteLine($"Error occurred while retrieving documents: {ex.Message}");
                Console.WriteLine($"Exception stack trace: {ex.StackTrace}");
                return new List<T>();
            }
        }

        public void AddEntry<T>(string collectionName, T entry)
        {
            var collection = _database.GetCollection<T>(collectionName);

            try
            {
                // Insert a new entry into the collection
                collection.InsertOne(entry);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log, throw)
                Console.WriteLine($"Error occurred while adding entry: {ex.Message}");
                Console.WriteLine($"Exception stack trace: {ex.StackTrace}");
            }
        }

        internal void DeleteEntry<T>(string collectionName, string id)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));

            // Delete the entry from the collection based on the provided ID
            collection.DeleteOne(filter);
        }
    }
}
>>>>>>> Stashed changes
