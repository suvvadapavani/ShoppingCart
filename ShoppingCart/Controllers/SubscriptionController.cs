using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Services;
using ShoppingCart.Services.ServiceInterfaces;

namespace ShoppingCart.Controllers
{
    public class SubscriptionController : Controller
    {
        IPaymentService payment = new StripeService();
        public IActionResult Index()
        {
            return View();
        }
    }
}
