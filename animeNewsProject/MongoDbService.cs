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
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("myDatabase");
        }

        // Add methods for your specific database operations

        // Example method: Get all documents from a collection
        public List<T> GetAllDocuments<T>(string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(_ => true).ToList();
        }
    }
}

