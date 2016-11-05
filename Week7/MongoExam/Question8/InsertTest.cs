namespace Question8
{
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;

    class InsertTest
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("test");

            var animals = db.GetCollection<BsonDocument>("animals");

            var animal = new BsonDocument
                            {
                            {"animal", "monkey"}
                            };

            await animals.InsertOneAsync(animal);   //inserts one here AND adds the recently created ObjectId to animal BsonDocument
            animal.Remove("animal");                //removes {"animal", "monkey"} from animal BsonDocument
            animal.Add("animal", "cat");            //adds {"animal", "cat"} to animal BsonDocument
            await animals.InsertOneAsync(animal);   //E11000 duplicate key error
            animal.Remove("animal");
            animal.Add("animal", "lion");
            await animals.InsertOneAsync(animal);
        }
    }
}
