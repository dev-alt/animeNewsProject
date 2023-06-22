using MongoDB.Bson;
using MongoDB.Driver;

namespace animeNewsProject.Pages
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MongoDbService _mongoDbService;


        public ArticleRepository(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<AnimeArticle> GetArticleByIdAsync(string id)
        {
            var articles = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");
            var article = articles.FirstOrDefault(a => a.DocumentId == id);
            return await Task.FromResult(article!);
        }

        public Task DeleteArticleByIdAsync(string id)
        {
            _mongoDbService.DeleteEntry<AnimeArticle>("articles", id);
            return Task.CompletedTask;
        }

        public async Task DeleteCommentAsync(string articleId, string comment)
        {
            var collection = _mongoDbService.GetCollection<AnimeArticle>("articles");
            var filter = Builders<AnimeArticle>.Filter.Eq("_id", ObjectId.Parse(articleId));

            // Create an update definition to remove the comment from the Comments array
            var update = Builders<AnimeArticle>.Update.Pull(a => a.Comments, comment);

            // Perform the update operation
            await collection.UpdateOneAsync(filter, update);
        }



        public async Task UpdateArticleAsync(AnimeArticle article)
        {
            var articles = _mongoDbService.GetAllDocuments<AnimeArticle>("articles");
            var existingArticle = articles.FirstOrDefault(a => a.DocumentId == article.DocumentId);

            if (existingArticle == null)
            {
                // Article not found
                return;
            }

            // Update the properties of the existing article if they exist in the provided article object
            if (!string.IsNullOrEmpty(article.Title))
                existingArticle.Title = article.Title;

            if (!string.IsNullOrEmpty(article.Text))
                existingArticle.Text = article.Text;

            if (article.DatePublished != null)
                existingArticle.DatePublished = article.DatePublished;

            if (article.Rating != 0)
                existingArticle.Rating = article.Rating;

            if (!string.IsNullOrEmpty(article.AuthorName))
                existingArticle.AuthorName = article.AuthorName;

            if (!string.IsNullOrEmpty(article.SourceLink))
                existingArticle.SourceLink = article.SourceLink;

            if (!string.IsNullOrEmpty(article.Category))
                existingArticle.Category = article.Category;

            if (article.Views != 0)
                existingArticle.Views = article.Views;

            if (!string.IsNullOrEmpty(article.Image))
                existingArticle.Image = article.Image;

            if (!string.IsNullOrEmpty(article.Summary))
                existingArticle.Summary = article.Summary;

            if (article.Tags != null)
                existingArticle.Tags = article.Tags;

            if (article.Comments != null)
                existingArticle.Comments = article.Comments;


            var collection = _mongoDbService.GetCollection<AnimeArticle>("articles");

            var filter = Builders<AnimeArticle>.Filter.Eq("_id", ObjectId.Parse(existingArticle.DocumentId));
            await collection.ReplaceOneAsync(filter, existingArticle);
        }
    }
}
