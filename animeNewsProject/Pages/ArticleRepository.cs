using animeNewsProject;
using animeNewsProject.Pages;
using MongoDB.Bson;
using MongoDB.Driver;

namespace animeNewsProject
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
            return await Task.FromResult(article);
        }





    }
}