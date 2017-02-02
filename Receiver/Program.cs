using System;
using Common;
using NServiceBus;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Receiver";

            var config = new BusConfiguration();
            config.EndpointName("Receiver");
            config.UseTransport<AzureStorageQueueTransport>();
            config.UsePersistence<InMemoryPersistence>();
            config.Conventions().DefiningCommandsAs(t => t == typeof(SomeMessage));

            using (var bus = Bus.Create(config).Start())
            {
                Console.WriteLine("Receiver Started. Press [ENTER] to close");
                Console.ReadLine();
            }

        }
    }
}
