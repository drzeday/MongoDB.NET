namespace Data
{
    using System.Collections.Generic;
    using MongoDB.Bson.Serialization.Attributes;

    public class Image
    {
        [BsonElement("_id")]
        public int Id { get; set; }

        [BsonElement("height")]
        public int Height { get; set; }

        [BsonElement("width")]
        public int Width { get; set; }

        [BsonElement("tags")]
        public IEnumerable<string> Tags { get; set; }
    }
}
