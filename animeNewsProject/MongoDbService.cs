using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace animeNewsProject
{
    public class MongoDbService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDbService(IMongoClient client, string databaseName)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _database = _client.GetDatabase(databaseName);

        }
        public IMongoClient GetMongoClient()
        {
            return _client;
        }
        // Add methods for your specific database operations

        // Example method: Get all documents from a collection
        public List<T> GetAllDocuments<T>(string articles)
        {
            var collection = _database.GetCollection<T>(articles);

            try
            {
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
    }
}