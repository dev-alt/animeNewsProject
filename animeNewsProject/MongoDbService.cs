using MongoDB.Bson;
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
        public List<T> GetAllDocuments<T>(string collectionName)
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

        public void AddEntry<T>(string articles, T entry)
        {
            var collection = _database.GetCollection<T>(articles);

            try
            {
                collection.InsertOne(entry);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log, throw, or return an empty list)
                Console.WriteLine($"Error occurred while adding entry: {ex.Message}");
                Console.WriteLine($"Exception stack trace: {ex.StackTrace}");
            }
        }

        internal void DeleteEntry<T>(string collectionName, string id)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.DeleteOne(filter);
        }
    }
}