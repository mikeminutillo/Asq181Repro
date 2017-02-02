using System;
using Common;
using NServiceBus;

namespace Receiver
{
    class SomeMessageHandler : IHandleMessages<SomeMessage>
    {
        public void Handle(SomeMessage message)
        {
            Console.WriteLine($"MSG: {message.SomeProperty}");
        }
    }
}