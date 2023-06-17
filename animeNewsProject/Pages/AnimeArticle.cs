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

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("date_published")]
        public DateTime? DatePublished { get; set; }

        [BsonElement("rating")]
        public double Rating { get; set; }

        [BsonElement("author_name")]
        public string? AuthorName { get; set; }

        [BsonElement("author_id")]
        public int? AuthorId { get; set; }

        [BsonElement("source_id")]
        public int? SourceId { get; set; }

        [BsonElement("SourceLink")]
        public string? SourceLink { get; set; }

        [BsonElement("category")]
        public string? Category { get; set; }

        [BsonElement("views")]
        public int Views { get; set; }

        [BsonElement("image")]
        public string? Image { get; set; }

        // Additional properties
        [BsonIgnoreIfNull]
        [BsonElement("summary")]
        public string? Summary { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("tags")]
        public string[] Tags { get; set; }
    }
}
