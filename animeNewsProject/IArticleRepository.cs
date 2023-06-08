using animeNewsProject.Pages;

namespace animeNewsProject
{

    public interface IArticleRepository
    {
        Task<AnimeArticle> GetArticleByIdAsync(string id);
        // Add additional methods to implement if needed
    }


}
