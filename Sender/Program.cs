using System;
using Common;
using NServiceBus;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sender";

            var config = new BusConfiguration();
            config.EndpointName("Sender");
            config.UseTransport<AzureStorageQueueTransport>();
            config.UsePersistence<InMemoryPersistence>();
            config.Conventions().DefiningCommandsAs(t => t == typeof(SomeMessage));

            using (var bus = Bus.CreateSendOnly(config))
            {
                for (var i = 0; i < 10; i++)
                {
                    bus.Send<SomeMessage>(m => m.SomeProperty = $"{i}");
                }
                Console.WriteLine("Sender Started. Press [ENTER] to close");
                Console.ReadLine();
            }
        }
    }
}
