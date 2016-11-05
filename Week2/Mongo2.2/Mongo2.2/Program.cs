using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo2._2
{
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
            var db = client.GetDatabase("students");
            
            //If you select homework grade-documents, 
            //sort by student and then by score, 
            //you can iterate through and find the lowest score for each student by noticing a change in student id.
            //As you notice that change of student_id, remove the document.

            var grades = await db.GetCollection<Grade>("grades")
                .Find(Builders<Grade>.Filter.Eq(g => g.Type, "homework"))
                .Sort(Builders<Grade>.Sort.Ascending(g => g.StudentId).Ascending(g => g.Score))
                .ToListAsync();

            var studentId = -1;
            var docsToRemove = new List<ObjectId>();
            foreach (var grade in grades)
            {
                if(studentId != grade.StudentId)
                {
                    studentId = grade.StudentId;
                    docsToRemove.Add(grade.Id);
                }
            }

            var result = await db.GetCollection<Grade>("grades")
                .DeleteManyAsync(Builders<Grade>.Filter.In(g => g.Id, docsToRemove));
        }
    }
}
