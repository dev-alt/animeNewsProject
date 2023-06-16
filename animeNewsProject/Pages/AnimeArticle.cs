using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace animeNewsProject.Pages
{
    public class AnimeArticle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the article.
        /// </summary>
        [BsonElement("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the article.
        /// </summary>
        [BsonElement("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the text of the article.
        /// </summary>
        [BsonElement("text")]
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the date when the article was published.
        /// </summary>
        [BsonElement("date_published")]
        public DateTime? DatePublished { get; set; }

        /// <summary>
        /// Gets or sets the rating of the article.
        /// </summary>
        [BsonElement("rating")]
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the ID of the author of the article.
        /// </summary>
        [BsonElement("author_id")]
        public int? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the source of the article.
        /// </summary>
        [BsonElement("source_id")]
        public int? SourceId { get; set; }

        /// <summary>
        /// Gets or sets the category of the article.
        /// </summary>
        [BsonElement("category")]
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets the number of views of the article.
        /// </summary>
        [BsonElement("views")]
        public int Views { get; set; }

        /// <summary>
        /// Gets or sets the image associated with the article.
        /// </summary>
        [BsonElement("image")]
        public string? Image { get; set; }
    }
}
