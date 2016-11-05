namespace Question1
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using MongoDB.Driver;

    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.ReadLine();

            //mongorestore --port 27017 -d enron -c messages messages.bson
            //db.messages.find({"headers.From":"andrew.fastow@enron.com", "headers.To":"jeff.skilling@enron.com"}).count()
        }

        private static async Task MainAsync()
        {
            var messages = await MongoContext.Messages.Find(Builders<Message>.Filter.Empty).ToListAsync();

            var query = messages
                .Where(
                    m =>
                    m.Headers.From == "andrew.fastow@enron.com"
                    //&& m.Headers.To.Contains("john.lavorato@enron.com"))
                    && m.Headers.To.Contains("jeff.skilling@enron.com"))
                 .ToList();

            Console.WriteLine(query.Count);
        }
    }
}
