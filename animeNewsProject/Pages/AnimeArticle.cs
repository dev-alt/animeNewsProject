using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace animeNewsProject.Pages
{
    public class AnimeArticle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? _Id { get; set; }

        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("date_published")]
        public DateTime? DatePublished { get; set; }


        [BsonElement("rating")]
        public double Rating { get; set; }

        [BsonElement("author_id")]
        public int? AuthorId { get; set; }

        [BsonElement("source_id")]
        public int? SourceId { get; set; }

        [BsonElement("category")]
        public string? Category { get; set; }

        [BsonElement("views")]
        public int Views { get; set; }


    }
}
