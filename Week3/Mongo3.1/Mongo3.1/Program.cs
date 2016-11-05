namespace Mongo3._1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;

    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.WriteLine("Press enter...");
            Console.ReadLine();
        }

        public static async Task MainAsync()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("school");

            //With the new schema, this problem is a lot harder and that is sort of the point.
            //One way is to find the lowest homework in code 
            //and then update the scores array with the low homework pruned.

            var students = await db.GetCollection<Student>("students")
                .Find(new BsonDocument())
                .ToListAsync();

            foreach (var student in students)
            {
                var scores = student.Scores.ToList();
                var lowestHomeworkScore = scores.Where(s => s.Type == "homework").OrderBy(s => s.Score).First();
                scores.Remove(lowestHomeworkScore);

                await db.GetCollection<Student>("students")
                    .UpdateOneAsync(
                        Builders<Student>.Filter.Eq(st => st.Id, student.Id),
                        Builders<Student>.Update.Set(st => st.Scores, scores));
            }
        }
    }
}
