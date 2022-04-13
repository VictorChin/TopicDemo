using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace SubscriptionProcessor
{
    class Program
    {
        static string connectionString =
                             "Endpoint=sb://vc0413.servicebus.windows.net/;SharedAccessKeyName=vcsub;SharedAccessKey=WBKuR5avcSD8C/1R1Dke4XyCmTBZxf0YWRfYcN9Z5dg=;EntityPath=demotopic";
        static string topicName = "demotopic";

        // name of the subscription to the topic
        static string subscriptionName = "vcsub";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;

        // the processor that reads and processes messages from the subscription
        static ServiceBusProcessor processor;
        static async Task Main(string[] args)
        {
            client = new ServiceBusClient(connectionString);
            processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
            await processor.StartProcessingAsync();
            Console.WriteLine("Wait for a minute and then press any key to end the processing");
            Console.ReadKey();

            // stop processing 
            Console.WriteLine("\nStopping the receiver...");
            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");
        }
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body} from subscription: {subscriptionName}");

            // complete the message. messages is deleted from the subscription. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
