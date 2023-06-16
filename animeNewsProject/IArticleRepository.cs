using animeNewsProject.Pages;

namespace animeNewsProject
{
    public interface IArticleRepository
    {

        Task<AnimeArticle> GetArticleByIdAsync(string id);
        Task DeleteArticleByIdAsync(string id);
        Task UpdateArticleAsync(AnimeArticle article);
    }
}