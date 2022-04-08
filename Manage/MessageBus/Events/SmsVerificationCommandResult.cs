namespace MessageBus.Events
{
    public class SmsVerificationCommandResult: ISmsVerificationCommandResult
    {
        public bool SmsSent { get; set; }
    }
}