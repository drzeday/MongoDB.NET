namespace Mongo5._1
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Comment
    {
        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }
    }
}
