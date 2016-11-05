namespace Mongo5._1
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Post
    {
        public ObjectId Id { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("permalink")]
        public string Permalink { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("tags")]
        public IEnumerable<string> Tags { get; set; }

        [BsonElement("comments")]
        public IEnumerable<Comment> Comments { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }
    }
}
