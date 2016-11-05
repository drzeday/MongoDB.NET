namespace Question3
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

            //db.messages.updateOne({"headers.Message-ID":"<8147308.1075851042335.JavaMail.evans@thyme>"},{$addToSet:{"headers.To":"mrpotatohead@mongodb.com"}})
            //mongo final3-validate-mongo-shell.js
        }

        public static async Task MainAsync()
        {
            var messages = await MongoContext.Messages.Find(Builders<Message>.Filter.Empty).ToListAsync();

            var find = messages.Where(m => m.Headers.MessageId.Equals("<8147308.1075851042335.JavaMail.evans@thyme>")).ToList();

            if (find.Count == 1)
            {
                Console.WriteLine("only 1 document found, ok so far.");
            }
            else
            {
                Console.WriteLine("something went wrong...");
                return;
            }

            var updatedDocument = find.Single();

            var insertedEmail = updatedDocument.Headers.To.SingleOrDefault(to => to == "mrpotatohead@mongodb.com");

            if (!string.IsNullOrEmpty(insertedEmail))
            {
                Console.WriteLine("DONE!");
            }
            else
            {
                Console.WriteLine("something went wrong...");
            }
        }
    }
}
