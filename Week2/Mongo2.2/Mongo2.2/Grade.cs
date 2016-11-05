﻿namespace Mongo2._2
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Grade
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("student_id")]
        public int StudentId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("score")]
        public double Score { get; set; }
    }
}
