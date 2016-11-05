namespace Question2
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

            // solution 1
            // db.messages.aggregate([ {$unwind:"$headers.To"} , {$group:{_id:"$_id",From:{$first:"$headers.From"},To:{$addToSet:"$headers.To"}}} , {$unwind:"$To"} , {$group:{_id:{from:"$From", to:"$To"}, messageCount:{$sum:1}}} , {$sort:{messageCount:-1}}])
            /*
            (step by step explanation)
            db.messages.aggregate(
                [ 
                    -> unwind on headers.To, converting each document to 1*nRecipients
                    {$unwind:"$headers.To"} , 
        
                    -> group by id (which at this point is repeated 1*nRecipients times) and merging the recipients with addToSet to get rid of repetitions in the recipients array
                    {$group:{_id:"$_id",From:{$first:"$headers.From"},To:{$addToSet:"$headers.To"}}} ,

                    -> unwinding again, this time on the recently created array set
                    {$unwind:"$To"} , 
        
                    -> group by pairs {from,to} and count them
                    {$group:{_id:{from:"$From", to:"$To"}, messageCount:{$sum:1}}} , 
                    
                    -> sort the pairs by the number of ocurrences, descending
                    {$sort:{messageCount:-1}}
                ]
            )

            */


            // solution 2
            // db.messages.aggregate([{$project: { from: "$headers.From",to: "$headers.To"} },{$unwind: "$to"},{$group: { _id: { _id: "$_id", from: "$from", to: "$to" } } },{$group: { _id: { from: "$_id.from", to: "$_id.to" }, count: {$sum: 1} } },{$sort: { count: -1} },{$limit: 2}],{ allowDiskUse: true})
            /*
            (step by step explanation)
            db.messages.aggregate(
                [
                    -> make a projection with only the sender and recipients
                    {$project: {from: "$headers.From",to: "$headers.To"}},
                    
                    -> unwind on the recipients
                    {$unwind: "$to"},
        
                    -> group by a compound _id with the {id, sender and recipient}, thus, getting rid of recipients that appear more than once
                    {$group : { _id : { _id: "$_id", from: "$from", to: "$to" }}},
        
                    -> and now, grouping by pairs {from,to}, just like in the 1st solution
                    {$group : { _id : { from: "$_id.from", to: "$_id.to" }, count: {$sum :1}}},
                    
                    -> same old
                    {$sort : {count:-1}}
                ],

                -> this solution needs to use the disk though
                {allowDiskUse:true}
            )
            */
        }

        static object obj = new object();
        private static async Task MainAsync()
        {
            var messages = await MongoContext.Messages.Find(Builders<Message>.Filter.Empty).ToListAsync();

            var projections = new List<MessageProjection>();

            foreach (var message in messages)
            {
                var sender = message.Headers.From;
                if (message.Headers.To != null)
                {
                    var recipients = message.Headers.To.Distinct().ToList();

                    foreach (var recipient in recipients)
                    {
                        var projection = projections.SingleOrDefault(p => p.From == sender && p.To == recipient);
                        if (projection == null)
                        {
                            projections.Add(new MessageProjection { From = sender, To = recipient, Count = 1 });
                        }
                        else
                        {
                            projection.Count++;
                        }
                    }
                }
            };

            var top10 = projections.OrderByDescending(p => p.Count).Take(10);

            foreach (var proj in top10)
            {
                Console.WriteLine(string.Format("from: {0}, to: {1}, messageCount:{2}", proj.From, proj.To, proj.Count));
            }

        }
    }

    public class MessageProjection
    {
        public string From { get; set; }

        public string To { get; set; }

        public int Count { get; set; }
    }
}
