using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using NServiceBus;

namespace SenderV6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sender v6";
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            var config = new EndpointConfiguration("SenderV6");

            // This line prevents the other side from receiving the messages
            config.SendOnly();

            var transport = config.UseTransport<AzureStorageQueueTransport>();

            transport.ConnectionString("UseDevelopmentStorage=true");

            config.UsePersistence<InMemoryPersistence>();
            config.Conventions().DefiningCommandsAs(t => t == typeof(SomeMessage));
            config.SendFailedMessagesTo("error");

            transport.Routing().RouteToEndpoint(
                assembly: typeof(SomeMessage).Assembly,
                destination: "Receiver");

            var endpoint = await Endpoint.Start(config);

            await Task.WhenAll(
                Enumerable.Range(1, 10).Select(i => endpoint.Send<SomeMessage>(m => m.SomeProperty = $"V6 {i}"))
            );

            Console.WriteLine("Started V6. Press [ENTER] to close");
            Console.ReadLine();

            await endpoint.Stop();
        }
    }
}
