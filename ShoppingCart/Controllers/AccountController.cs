using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Services.ServiceInterfaces;

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        private readonly INotificationService notificationService;
        public AccountController(INotificationService notification)
        {
            notificationService=notification;
                
        }
        [HttpPost("Register")]
        public IActionResult Register(string email)
        {
            var result = notificationService.SendEmail(email, "Welcome"," Thanks for registering");
            return Ok(result);
        }
        [HttpPost("sendOtp")]
        public IActionResult SendOtp(string phoneNumber)
        {
            var result = notificationService.SendSms(phoneNumber, "Your otp is 1234");
            return Ok(result);
        }
      
    }
}
