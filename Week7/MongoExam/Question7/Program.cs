namespace Question7
{
    using System;
    using System.Collections.Generic;
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

            //mongoimport --drop -d test -c albums albums.json
            //mongoimport --drop -d test -c images images.json
            //db.albums.createIndex({"images":1})
            //db.images.createIndex({"tags":1})
        }

        private static async Task MainAsync()
        {
            var total = await MongoContext.Images.Find(Builders<Image>.Filter.Empty).CountAsync();
            Console.WriteLine("total images: " + total);

            await CountImagesWithTagSunrises();

            var orphans = new List<Image>();

            await MongoContext.Images.Find(Builders<Image>.Filter.Empty).ForEachAsync(async image =>
            {
                var album = await MongoContext.Albums.Find(
                    Builders<Album>.Filter.AnyEq(x => x.Images, image.Id)).FirstOrDefaultAsync();
                if (album == null)
                {
                    orphans.Add(image);
                }
            });

            var orphanIds = orphans.Select(img => img.Id).ToList();
            var deleted = await MongoContext.Images.DeleteManyAsync(
                Builders<Image>.Filter.In(x => x.Id, orphanIds));
            Console.WriteLine(deleted.DeletedCount + " orphans deleted");

            total = await MongoContext.Images.Find(Builders<Image>.Filter.Empty).CountAsync();
            Console.WriteLine(total + " images left in the collection");

            await CountImagesWithTagSunrises();
        }

        private static async Task CountImagesWithTagSunrises()
        {
            var sunrises = await MongoContext.Images.Find(
                Builders<Image>.Filter.AnyEq(x => x.Tags, "sunrises")).CountAsync();
            Console.WriteLine(sunrises + " images with tag sunrises");
        }
    }
}
