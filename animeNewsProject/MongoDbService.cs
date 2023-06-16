using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace animeNewsProject
{
    /// <summary>
    /// Service class for interacting with MongoDB.
    /// </summary>
    public class MongoDbService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbService"/> class.
        /// </summary>
        /// <param name="client">The MongoDB client.</param>
        /// <param name="databaseName">The name of the database.</param>
        public MongoDbService(IMongoClient client, string databaseName)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _database = _client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Gets the MongoDB client.
        /// </summary>
        /// <returns>The MongoDB client.</returns>
        public IMongoClient GetMongoClient()
        {
            return _client;
        }

        /// <summary>
        /// Gets all documents from a collection.
        /// </summary>
        /// <typeparam name="T">The type of documents to retrieve.</typeparam>
        /// <param name="articles">The name of the collection.</param>
        /// <returns>A list of documents.</returns>
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

        /// <summary>
        /// Gets the MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of documents in the collection.</typeparam>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>The MongoDB collection.</returns>
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Adds an entry to the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of the entry.</typeparam>
        /// <param name="articles">The name of the collection.</param>
        /// <param name="entry">The entry to add.</param>
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

        /// <summary>
        /// Deletes an entry from the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of the entry.</typeparam>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="id">The ID of the entry to delete.</param>
        internal void DeleteEntry<T>(string collectionName, string id)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
            collection.DeleteOne(filter);
        }
    }
}
