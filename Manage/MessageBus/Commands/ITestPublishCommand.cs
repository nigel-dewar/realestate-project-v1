using System;

namespace MessageBus.Commands
{
    public interface ITestPublishCommand
    {
        Guid TestPublishId { get; set; }
        string TestMessage { get; set; }
    }
}