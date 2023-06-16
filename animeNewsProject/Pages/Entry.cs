using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace animeNewsProject.Pages
{
    /// <summary>
    /// Represents an entry in the news collection.
    /// </summary>
    public class Entry
    {
        /// <summary>
        /// Gets or sets the document ID.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the entry.
        /// </summary>
        [BsonElement("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the entry.
        /// </summary>
        [BsonElement("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the text content of the entry.
        /// </summary>
        [BsonElement("text")]
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the date when the entry was published.
        /// </summary>
        [BsonElement("date_published")]
        public DateTime? DatePublished { get; set; }

        /// <summary>
        /// Gets or sets the rating of the entry.
        /// </summary>
        [BsonElement("rating")]
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the ID of the author.
        /// </summary>
        [BsonElement("author_id")]
        [BsonRepresentation(BsonType.Int32)]
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the source.
        /// </summary>
        [BsonElement("source_id")]
        [BsonRepresentation(BsonType.Int32)]
        public int SourceId { get; set; }

        /// <summary>
        /// Gets or sets the category of the entry.
        /// </summary>
        [BsonElement("category")]
        public string? Category { get; set; }

        /// <summary>
        /// Gets or sets the number of views of the entry.
        /// </summary>
        [BsonElement("views")]
        public int Views { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the entry.
        /// </summary>
        [BsonElement("image")]
        public string? Image { get; set; }
    }
}
