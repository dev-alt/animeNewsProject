using animeNewsProject.Pages;

namespace animeNewsProject
{
    /// <summary>
    /// Interface for accessing and manipulating articles.
    /// </summary>
    public interface IArticleRepository
    {
        /// <summary>
        /// Gets an article by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the article.</param>
        /// <returns>The article with the specified ID.</returns>
        Task<AnimeArticle> GetArticleByIdAsync(string id);

        /// <summary>
        /// Deletes an article by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the article to delete.</param>
        Task DeleteArticleByIdAsync(string id);

        /// <summary>
        /// Updates an article asynchronously.
        /// </summary>
        /// <param name="article">The updated article.</param>
        Task UpdateArticleAsync(AnimeArticle article);
    }
}