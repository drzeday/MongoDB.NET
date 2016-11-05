namespace Data
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class Header
    {
        [BsonElement("Content-Transfer-Encoding")]
        public string ContentTransferEncoding { get; set; }

        [BsonElement("Content-Type")]
        public string ContentType { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("From")]
        public string From { get; set; }

        [BsonElement("Message-ID")]
        public string MessageId { get; set; }

        [BsonElement("Mime-Version")]
        public string MimeVersion { get; set; }

        [BsonElement("Subject")]
        public string Subject { get; set; }

        [BsonElement("To")]
        public IEnumerable<string> To { get; set; }

        [BsonElement("X-FileName")]
        public string XFileName { get; set; }

        [BsonElement("X-Folder")]
        public string XFolder { get; set; }

        [BsonElement("X-From")]
        public string XFrom { get; set; }
        
        [BsonElement("X-Origin")]
        public string XOrigin { get; set; }

        [BsonElement("X-To")]
        public string XTo { get; set; }

        [BsonElement("X-bcc")]
        public string Xbcc { get; set; }

        [BsonElement("X-cc")]
        public string Xcc { get; set; }
    }
}