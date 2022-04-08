namespace MessageBus.Events
{
    public interface ISmsVerificationCommandResult
    {
        bool SmsSent { get; set; }
    }
}