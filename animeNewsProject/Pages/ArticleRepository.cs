using animeNewsProject;
using animeNewsProject.Pages;
using MongoDB.Bson;
using MongoDB.Driver;

namespace animeNewsProject
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MongoDbService _mongoDbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleRepository"/> class.
        /// </summary>
        /// <param name="mongoDbService">The MongoDB service.</param>
        public ArticleRepository(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        /// <summary>
        /// Gets an article by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the article.</param>
        /// <returns>The article with the specified ID.</returns>
        public async Task<AnimeArticle> GetArticleByIdAsync(string id)
        {
            var articles = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");
            var article = articles.FirstOrDefault(a => a.DocumentId == id);
            return await Task.FromResult(article!);
        }

        /// <summary>
        /// Deletes an article by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the article to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task DeleteArticleByIdAsync(string id)
        {
            _mongoDbService.DeleteEntry<AnimeArticle>("articles", id);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an article asynchronously.
        /// </summary>
        /// <param name="article">The updated article.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateArticleAsync(AnimeArticle article)
        {
            var articles = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");
            var existingArticle = articles.FirstOrDefault(a => a.DocumentId == article.DocumentId);

            if (existingArticle == null)
            {
                // Article not found
                return;
            }

            // Update the properties of the existing article
            existingArticle.Title = article.Title;
            existingArticle.Text = article.Text;
            existingArticle.DatePublished = article.DatePublished;
            existingArticle.Rating = article.Rating;
            existingArticle.AuthorId = article.AuthorId;
            existingArticle.SourceId = article.SourceId;
            existingArticle.Category = article.Category;
            existingArticle.Views = article.Views;

            var collection = _mongoDbService.GetCollection<AnimeArticle>("articles");

            var filter = Builders<AnimeArticle>.Filter.Eq("_id", ObjectId.Parse(existingArticle.DocumentId));
            await collection.ReplaceOneAsync(filter, existingArticle);
        }
    }
}
