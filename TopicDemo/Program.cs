using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TopicDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = 
                "Endpoint=sb://vc0413.servicebus.windows.net/;SharedAccessKeyName=sender;SharedAccessKey=PTQTOgkIKw6IMHDkmIjzyN8seH/dfs/vmBK1kxutrik=;EntityPath=demotopic";
            string topicName = "demotopic";

            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topicName);
            ServiceBusMessage message = new ServiceBusMessage("Hello From Victor");
            message.ApplicationProperties.Add("Priority", "1");

            await sender.SendMessageAsync(message);

            IList<ServiceBusMessage> aBatch = new List<ServiceBusMessage>();

            for (int i = 0; i < 10; i++)
            {
                var msg = new ServiceBusMessage($"Hello From Victor-{i}");
                msg.ApplicationProperties.Add("OrderNumber", i);
                aBatch.Add(msg);
            }
            await sender.SendMessagesAsync(aBatch);
        }
    }
}
