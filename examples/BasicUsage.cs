using System;
using Reach;
using Reach.Rest.Api.Messaging;
using System.Collections.Generic;

namespace Basic.Usage
{
    class BasicUsage
    {
        private static string apiUser = Environment.GetEnvironmentVariable("REACH_TALKYLABS_API_USER");
        private static string apiKey = Environment.GetEnvironmentVariable("REACH_TALKYLABS_API_KEY");
        private static string srcPhoneNumber = Environment.GetEnvironmentVariable("SRC_PHONE_NUMBER");
        private static string destPhoneNumber = Environment.GetEnvironmentVariable("DEST_PHONE_NUMBER");

        public static int size<T>(IEnumerable<T> data) {

            if (data == null)
            {
                return 0;
            }
            ICollection<T> is2 = data as ICollection<T>;
            if (is2 != null)
            {
                return is2.Count;
            }
            int counter = 0;
            foreach (T i in data) {
                counter++;
            }
            return counter;
        }


        static void Main(string[] args)
        {
            ReachClient.Init(apiUser, apiKey);

            // Get all messages
    	    var messagingItems = MessagingItemResource.Read();
    	    Console.WriteLine("There are " + size(messagingItems) + " messages in your account.");

            // Get only last 10 messages...
            var someMessagingItems = MessagingItemResource.Read(limit:10);
            Console.WriteLine("Here are the last 10 messages in your account:");
            foreach(var msgItem in someMessagingItems) {
                Console.WriteLine(msgItem);
            }

            // Get messages in smaller pages...
            var messagingItems2 = MessagingItemResource.Read(pageSize: 10);
            Console.WriteLine("There are " + size(messagingItems2) + " messages in your account.");

            Console.WriteLine("Sending a message...");
            var messagingItem = MessagingItemResource.Send(
                dest: destPhoneNumber,
                src: srcPhoneNumber,
                body: "Reach is the best!"
            );
            Console.WriteLine(messagingItem.MessageId);
            Console.WriteLine(messagingItem);

        }
    }
}
