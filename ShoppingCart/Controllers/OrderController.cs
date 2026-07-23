using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Services;
using ShoppingCart.Services.ServiceInterfaces;

namespace ShoppingCart.Controllers
{
    public class OrderController : Controller
    {
        IPaymentService payment=new StripeService();
        public IActionResult Index()
        {
            string result = payment.Pay(1000);
            return View();
        }
    }
}
