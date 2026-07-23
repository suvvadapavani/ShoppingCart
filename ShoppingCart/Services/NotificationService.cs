using ShoppingCart.Services.ServiceInterfaces;

namespace ShoppingCart.Services
{
    public class NotificationService:INotificationService
    {
        string INotificationService.SendEmail(string to, string subject, string body)
        {
            return $"Email sent to {to} with subjcet '{subject}'";

        }

        string INotificationService.SendSms(string phoneNumber, string message)
        {
            return $"SMS sent to {phoneNumber}:{message}";
        }

        string INotificationService.SendPush(string DeviceId, string message)
        {
            return $"Push Notification sent to device {DeviceId}:{message}";
        }

      
    }
}
