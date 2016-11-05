namespace Mongo3._1
{
    using MongoDB.Bson.Serialization.Attributes;

    public class Grade
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("score")]
        public double Score { get; set; }
    }
}
