using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace animeNewsProject.Pages
{
    public class Entry
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

        [BsonElement("SourceLink")]
        public string? SourceLink { get; set; }

        [BsonElement("category")]
        public string? Category { get; set; }

        [BsonDefaultValue(0)]
        [BsonElement("views")]
        public int Views { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("image")]
        public string? Image { get; set; }

        // Additional properties
        [BsonIgnoreIfNull]
        [BsonElement("summary")]
        public string? Summary { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("tags")]
        public string[]? Tags { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("comments")]
        public List<string>? Comments { get; set; }
    }

    public class Author
    {
        [BsonElement("id")]
        [BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }
    }
}