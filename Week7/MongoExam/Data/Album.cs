namespace Data
{
    using System.Collections.Generic;
    using MongoDB.Bson.Serialization.Attributes;

    public class Album
    {
        [BsonElement("_id")]
        public int Id { get; set; }

        [BsonElement("images")]
        public IEnumerable<int> Images { get; set; }
    }
}
