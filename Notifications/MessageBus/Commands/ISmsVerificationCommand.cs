namespace MessageBus.Commands
{
    public interface ISmsVerificationCommand
    {
        string Number { get; set; }
        string Code { get; set; }
    }
}