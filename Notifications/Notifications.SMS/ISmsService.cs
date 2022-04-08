namespace Notifications.SMS
{
    public interface ISmsService
    {
        bool SendVerificationCode(string number, string message);
    }
}