namespace M101DotNet.WebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Post
    {
        // XXX WORK HERE
        // add in the appropriate properties for a post
        // The homework instructions contain the schema.
        public Post()
        {
            this.Tags = new List<string>();
            this.Comments = new List<Comment>();
        }

        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        
        public string Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}