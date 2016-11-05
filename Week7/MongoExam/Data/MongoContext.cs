namespace Data
{
    using System.Configuration;
    using MongoDB.Driver;

    public static class MongoContext
    {
        private static MongoUrl url;
        private static IMongoClient client;
        private static IMongoDatabase database;

        static MongoContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString;
            url = new MongoUrl(connectionString);
            client = new MongoClient(url);
            database = client.GetDatabase(url.DatabaseName);
        }

        public static IMongoCollection<Message> Messages
        {
            get
            {
                return database.GetCollection<Message>("messages");
            }
        }
        
        public static IMongoCollection<Album> Albums
        {
            get
            {
                return database.GetCollection<Album>("albums");
            }
        }

        public static IMongoCollection<Image> Images
        {
            get
            {
                return database.GetCollection<Image>("images");
            }
        }
    }
}
