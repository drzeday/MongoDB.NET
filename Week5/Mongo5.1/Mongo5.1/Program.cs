namespace Mongo5._1
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
            Console.ReadLine();
        }

        private static async Task MainAsync()
        {
            var url = new MongoUrl("mongodb://localhost:27017/blog");
            var client = new MongoClient(url);
            var context = client.GetDatabase(url.DatabaseName);
            var collection = context.GetCollection<Post>("posts");

            var posts = await collection.Find(Builders<Post>.Filter.Empty).ToListAsync();
            var commentsCountPerAuthor = new Dictionary<string, int>();

            // ITERATING
            foreach (var post in posts)
            {
                foreach (var comment in post.Comments)
                {
                    if (commentsCountPerAuthor.ContainsKey(comment.Author))
                    {
                        commentsCountPerAuthor[comment.Author]++;
                    }
                    else
                    {
                        commentsCountPerAuthor[comment.Author] = 1;
                    }
                }
            }
            
            Console.WriteLine("The author with the greatest number of comments is:");
            var item = commentsCountPerAuthor.OrderByDescending(pair => pair.Value).First();
            Console.WriteLine(string.Format("author : {0} , commentsCount : {1}", item.Key, item.Value));

            // .NET MONGO AGGREGATION
            var agg = collection.Aggregate()
                .Unwind(p => p.Comments)
                .Group(p => p["comments.author"], g => new { Author = g.Key, CommentsCount = g.Sum(p => 1) })
                .SortByDescending(g1 => g1.CommentsCount);
            Console.WriteLine((await agg.FirstOrDefaultAsync()));
        }
    }
}
