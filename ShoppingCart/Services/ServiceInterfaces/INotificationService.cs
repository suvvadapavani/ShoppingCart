using System.Collections.Specialized;

namespace ShoppingCart.Services.ServiceInterfaces
{
    public interface INotificationService
    {
        string SendEmail(string to,string subject,string body);
        string SendSms(string phoneNumber,string message);
        string SendPush(string DeviceId, string message);
    }
}
