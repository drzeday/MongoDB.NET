using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Mongo4._1
{
    class Program
    {
        static string[] Brands = new[] { "GE", "Armani", "Chloe", "Dolce&Gabana", "Givenchy", "SaintLaurent", "Valentino" };
        static string[] Authors = new[] { "Luis", "Daniel", "Hugo", "Jose", "Joao", "Carla", "Filipe" };

        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.WriteLine("done!");
            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var context = client.GetDatabase("store");
            var collection = context.GetCollection<Product>("products");

            var products = new List<Product>();
            var rnd = new Random();
            var nBrands = Brands.Count();
            var nAuthors = Authors.Count();
            for (int i = 1; i <= 100000; i++)
            {
                var nReviews = rnd.Next(4);
                var reviews = new List<Review>();
                for (int j = 0; j < nReviews; j++)
                {
                    reviews.Add(new Review { Author = Authors[rnd.Next(nAuthors)] });
                }

                products.Add(
                    new Product
                    {
                        SKU = Guid.NewGuid().ToString(),
                        Price = rnd.Next(1, 101),
                        Description = string.Concat("product_", i),
                        Category = rnd.Next(10),
                        Brand = Brands[rnd.Next(nBrands)],
                        Reviews = reviews
                    });
            }

            Console.WriteLine("Creating store.products...");
            await collection.InsertManyAsync(products);

            Console.Write("Creating unique index on sku ascending named...");
            Console.WriteLine(await collection.Indexes.CreateOneAsync(Builders<Product>.IndexKeys.Ascending(p => p.SKU), new CreateIndexOptions { Unique = true }));

            Console.Write("Creating index on price descending named...");
            Console.WriteLine(await collection.Indexes.CreateOneAsync(Builders<Product>.IndexKeys.Descending(p => p.Price)));

            Console.Write("Creating index on description ascending named...");
            Console.WriteLine(await collection.Indexes.CreateOneAsync(Builders<Product>.IndexKeys.Ascending(p => p.Description)));

            Console.Write("Creating index on category,brand ascending named...");
            Console.WriteLine(await collection.Indexes.CreateOneAsync(Builders<Product>.IndexKeys.Ascending(p => p.Category).Ascending(p => p.Brand)));

            Console.Write("Creating index on reviews.author ascending named...");
            Console.WriteLine(await collection.Indexes.CreateOneAsync(Builders<Product>.IndexKeys.Ascending("reviews.author")));
        }
    }
    public class Product
    {
        public ObjectId _id { get; set; }

        [BsonElement("sku")]
        public string SKU { get; set; }

        [BsonElement("price")]
        public int Price { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("category")]
        public int Category { get; set; }

        [BsonElement("brand")]
        public string Brand { get; set; }

        [BsonElement("reviews")]
        public IEnumerable<Review> Reviews { get; set; }
    }

    public class Review
    {
        [BsonElement("author")]
        public string Author { get; set; }
    }
}
